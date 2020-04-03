using System;
using System.IO;
using System.Threading.Tasks;
using MicrosoftCSA.HelperClasses;
using SB_Cosmo_BlobStorage_JobMonitor.DataClasses;

namespace SB_Cosmo_BlobStorage_JobMonitor
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("ServiceBus\t: Monitoring Queue {0}", ConnectionConfiguration.ServiceBusQueue);

            StorageBlobsHelper.Instance.ConnectToAzureStorageBlobs(ConnectionConfiguration.BlobStorageCS);
            CosmosHelper.Instance.ConnectToCosmosDB(ConnectionConfiguration.CosmosEndpointURL, ConnectionConfiguration.CosmosDBAuthKey);
            ServiceBusHelper.Instance.ConnectToServiceBus(ConnectionConfiguration.ServiceBusCS, ConnectionConfiguration.ServiceBusQueue, true);
            ServiceBusHelper.Instance.ServiceBusHelperException += Instance_ServiceBusHelperException;
            ServiceBusHelper.Instance.ServiceBusHelperReceiveMessage += Instance_ServiceBusHelperReceiveMessage;

            Console.ReadKey();
        }

        private static void Instance_ServiceBusHelperException(object sender, Exception ex)
        {
            ShowException(ex,"ServiceBus");
        }

        private async static void Instance_ServiceBusHelperReceiveMessage(object sender, string message)
        {
            Console.WriteLine("ServiceBus\t: Message Received contains Body: {0}", message);
            String[] parts = message.Split('|');
            ImageItem imageItem = GetCosmoRecordByIdAndPartitionKey<ImageItem>(parts[0], parts[1]);
            if (imageItem != null)
            {
                Console.WriteLine("StorageBlobsHelper\t: Attempting to download file {0} from blobstorage container {1} (URI=", imageItem.filename, imageItem.id, imageItem.blobURI);
                bool downloading = await StorageBlobsHelper.Instance.DownloadBlobToLocalStorage(imageItem.id, imageItem.filename, ConnectionConfiguration.LocalStorage);
                if (downloading)
                {
                    Console.WriteLine("Downloaded blob {0}", imageItem.filename);
                }
                else
                {
                    Console.WriteLine("StorageBlobsHelper\t: File Download skipped as file has not been written to blob storage yet. Not reprocessing");
                }
            }
        }

        private static T GetCosmoRecordByIdAndPartitionKey<T>(string id, string partitionKey)
        {
            Console.WriteLine("CosmosDB\t: Querying in {0}/{1} for document with id {2}", ConnectionConfiguration.CosmosDBName, ConnectionConfiguration.CosmosContainerName, id);

            try
            {
                T imageItem = CosmosHelper.Instance.GetItem<T>(ConnectionConfiguration.CosmosDBName, ConnectionConfiguration.CosmosContainerName, id, partitionKey);
                bool foundCosmosRecord = imageItem != null ? true : false;
                
                if (foundCosmosRecord)
                {
                    ImageItem stImageItem = imageItem as ImageItem;
                    Console.WriteLine("CosmosDB\t: Found Matching file {0} for id {1} with BlobUri of {2}", id, stImageItem.filename, stImageItem.blobURI);
                    return imageItem;
                }
                else
                {
                    Console.WriteLine("CosmosDB\t: Did not find a record for id {0}", id);
                }
            }
            catch(Exception ex)
            {
                ShowException(ex, "CosmosDB");
            }

            return default(T);
        }

        private static void ShowException(Exception ex, string componentName)
        {
            Console.WriteLine("{0}\t: EXCEPTION Thrown\n", componentName);
            Console.WriteLine("{0}\t: Exception Details Begin >>>>>>>\n\n{1}", componentName, ex.ToString());
            Console.WriteLine("{0}\t: Exception Details End <<<<<<<<<", componentName);
            Console.WriteLine("{0}\t: Exception was handled and program has resumed operation.", componentName);
        }
    }
}

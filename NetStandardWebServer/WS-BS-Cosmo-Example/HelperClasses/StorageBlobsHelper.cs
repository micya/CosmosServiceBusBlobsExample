using Azure;
using Azure.Storage.Blobs;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MicrosoftCSA.HelperClasses
{
    public sealed class StorageBlobsHelper
    {
        private static readonly Lazy<StorageBlobsHelper> lazy = new Lazy<StorageBlobsHelper>(() => new StorageBlobsHelper());

        public static StorageBlobsHelper Instance { get { return lazy.Value; } }

        private static BlobServiceClient blobServiceClient = null;

        private StorageBlobsHelper()
        {
        }

        public void ConnectToAzureStorageBlobs(string BlobStorageCS)
        {
            blobServiceClient = new BlobServiceClient(BlobStorageCS);
        }

        public async Task<String> WriteFileToBlobStorage(string containerName, string blobname, Stream fileContent)
        {
            BlobContainerClient containerClientTask = await blobServiceClient.CreateBlobContainerAsync(containerName);
            BlobClient blobClient = containerClientTask.GetBlobClient(blobname);
            await blobClient.UploadAsync(fileContent, true);
            return blobClient.Uri.AbsoluteUri;
        }

        public async Task<bool> DownloadBlobToLocalStorage(string containerName, string blobname, string localPath)
        {
            String outputmessage = String.Empty;
            BlobContainerClient containerClientTask = blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClientTask.GetBlobClient(blobname);
            if (await blobClient.ExistsAsync() == false)
            {
                return false;
            }
            else
            {
                await blobClient.DownloadToAsync(Path.Combine(localPath, blobname));
                return true;
            }
        }
    }
}
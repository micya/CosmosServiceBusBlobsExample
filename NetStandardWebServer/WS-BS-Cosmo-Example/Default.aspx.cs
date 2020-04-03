using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Configuration;
using MicrosoftCSA.HelperClasses;
using WS_BS_Cosmo_Example.DataClasses;

namespace WS_BS_Cosmo_Example
{
    public partial class _Default : Page
    {
        private static string cosmosDBName = WebConfigurationManager.AppSettings["CosmosDBName"];
        private static string cosmosContainerName = WebConfigurationManager.AppSettings["CosmosContainerName"];
        private static string cosmosPartitionKey = WebConfigurationManager.AppSettings["CosmosPartitionKey"];
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected async void UploadImageButton_Click(object sender, EventArgs e)
        {
            if (FileUpload.HasFile)
            {
                string filename = Path.GetFileName(FileUpload.FileName);
                Stream uploadedFileStream = FileUpload.FileContent;
                ImageItem image = new ImageItem();
                string bloburi = await StorageBlobsHelper.Instance.WriteFileToBlobStorage(image.id, filename, uploadedFileStream);
                image.blobURI = bloburi;
                image.filename = filename;
                await CosmosHelper.Instance.CreateItem<ImageItem>(cosmosDBName, cosmosContainerName, cosmosPartitionKey, image);
                await ServiceBusHelper.Instance.SendSBMessage(image.id + "|" + filename);
            }
        }
    }
}
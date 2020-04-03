using System;
using System.Web.Configuration;
using System.Web.UI;
using MicrosoftCSA.HelperClasses;

namespace WS_BS_Cosmo_Example
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ServiceBusHelper.Instance.ConnectToServiceBus(WebConfigurationManager.AppSettings["ServiceBusCS"], WebConfigurationManager.AppSettings["ServiceBusQ"]);
            CosmosHelper.Instance.ConnectToCosmosDB(WebConfigurationManager.AppSettings["CosmosEndPointURL"], WebConfigurationManager.AppSettings["CosmosDBAuthKey"]);
            StorageBlobsHelper.Instance.ConnectToAzureStorageBlobs(WebConfigurationManager.AppSettings["BlobStorageCS"]);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace SB_Cosmo_BlobStorage_JobMonitor
{
    public static class ConnectionConfiguration
    {
        public static readonly String ServiceBusCS = "";
        public static readonly String ServiceBusQueue = "imageprocessingsbqueue";

        public static readonly String BlobStorageCS = "";

        public static readonly String CosmosEndpointURL = "";
        public static readonly String CosmosDBAuthKey = "";

        public static readonly String CosmosDBName = "ImagesDataDB";
        public static readonly String CosmosContainerName = "ImageDataContainer";

        public static readonly String LocalStorage = "";
    }
}

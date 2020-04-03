using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SB_Cosmo_BlobStorage_JobMonitor.DataClasses
{
    [Serializable]
    public class ImageItem
    {
        public String id { get; set; } = Guid.NewGuid().ToString();
        public String filename { get; set; }
        public String blobURI { get; set; }
    }

}
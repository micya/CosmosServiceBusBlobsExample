using System;

namespace WS_BS_Cosmo_Example.DataClasses
{
    [Serializable]
    public class ImageItem
    {
        public String id { get; set;  } = Guid.NewGuid().ToString();
        public String filename { get; set; }
        public String blobURI { get; set; }
    }

}
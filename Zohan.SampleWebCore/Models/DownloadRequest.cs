using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Zohan.SampleWebCore.Models
{
    public class DownloadRequest
    {
        #region Properties

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        public string CustomerName { get; set; }

        public string RequestedFileName { get; set; }

        public DateTime RequestedDateTime { get; set; }

        public string RequestType { get; set; }

        #endregion
    }
}

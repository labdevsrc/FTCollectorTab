using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FTCollectorApp.Model.Reference
{
    public class ResponseKeyList
    {
        [JsonProperty("key1")]
        public string? key1 { get; set; }

        [JsonProperty("key2")]
        public string? key2 { get; set; }
        [JsonProperty("key3")]

        public string? key3 { get; set; }
        [JsonProperty("key4")]
        public string? key4 { get; set; }

        [JsonProperty("locatepointkey")]
        public string? locatepointkey { get; set; }

        [JsonProperty("status")]
        public string? status { get; set; }

    }
}

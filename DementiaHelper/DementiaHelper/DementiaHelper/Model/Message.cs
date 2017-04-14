using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DementiaHelper.Model
{
    [JsonObject]
    public class Message
    {
        [JsonProperty("MessageRecieved")]
        public string MessageRecieved { get; set; }
        [JsonProperty("MessageSent")]
        public string MessageSent { get; set; }
        [JsonProperty("MessageSentIsVisible")]
        public bool MessageSentIsVisible { get; set; }
        [JsonProperty("MessageRecievedIsVisible")]
        public bool MessageRecievedIsVisible { get; set; }

    }
}

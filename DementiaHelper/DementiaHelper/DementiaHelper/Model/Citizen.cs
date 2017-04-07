using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DementiaHelper.Model
{
    [JsonObject]
    public class Citizen
    {
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("CitizenId")]
        public int CitizenId { get; set; }
    }
}

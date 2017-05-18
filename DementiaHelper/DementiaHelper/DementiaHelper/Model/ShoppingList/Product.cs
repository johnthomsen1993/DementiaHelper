using Newtonsoft.Json;

namespace DementiaHelper.Model
{
    public class Product
    {
        [JsonProperty]
        public int? ProductId { get; set; }
        [JsonProperty]
        public string ProductName { get; set; }
    }
}

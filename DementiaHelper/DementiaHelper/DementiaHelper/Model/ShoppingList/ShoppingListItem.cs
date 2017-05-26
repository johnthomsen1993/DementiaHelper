using Newtonsoft.Json;

namespace DementiaHelper.Model
{
    public class ShoppingListItem
    {
        [JsonProperty]
        public int? ShoppingListItemId { get; set; }
        [JsonProperty]
        public int? CitizenId { get; set; }
        [JsonProperty]
        public int? Quantity { get; set; }
        [JsonProperty]
        public bool? Bought { get; set; }
        public Product Product { get; set; }
    }
}

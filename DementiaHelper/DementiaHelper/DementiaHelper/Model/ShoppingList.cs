using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;

namespace DementiaHelper.Model
{
    [JsonObject]
    public class ShoppingList
    {
        [JsonExtensionData]
        public ObservableCollection<ShoppingListItem> ShoppingListItems { get; set; }
    }
    
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
    
    public class Product
    {
        [JsonProperty]
        public int? ProductId { get; set; }
        [JsonProperty]
        public string ProductName { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DementiaHelper.Model
{
    [JsonObject]
    public class ShoppingList
    {
        [JsonExtensionData]
        public ObservableCollection<ShoppingListDetail> ShoppingListDetails { get; set; }
    }

    [JsonObject]
    public class ShoppingListDetail
    {
        [JsonProperty("quantity")]
        public int Quantity { get; set; }
        [JsonProperty("bought")]
        public bool Bought { get; set; }
        public Product Product { get; set; }
    }

    [JsonObject]
    public class Product
    {
        [JsonProperty("productname")]
        public string ProductName { get; set; }
    }
}

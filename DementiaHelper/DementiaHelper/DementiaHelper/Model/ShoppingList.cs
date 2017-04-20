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
        [JsonProperty]
        public int? ShoppingListId { get; set; }
        [JsonExtensionData]
        public ObservableCollection<ShoppingListDetail> ShoppingListDetails { get; set; }
    }
    
    public class ShoppingListDetail
    {
        [JsonProperty]
        public int? ShoppingListDetailId { get; set; }
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

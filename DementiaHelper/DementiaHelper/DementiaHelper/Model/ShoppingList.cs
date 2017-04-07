using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DementiaHelper.Model
{
    public class ShoppingList
    {
        public ObservableCollection<ShoppingListDetail> ShoppingListDetails { get; set; }
    }
    
    public class ShoppingListDetail
    {
        public int? ShoppingListDetailId { get; set; }
        public int? Quantity { get; set; }
        public bool? Bought { get; set; }
        public Product Product { get; set; }
    }
    
    public class Product
    {
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
    }
}

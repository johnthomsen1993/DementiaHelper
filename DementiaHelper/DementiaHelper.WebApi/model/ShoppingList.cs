using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DementiaHelper.WebApi.model
{
    public class ShoppingList
    {
        public int ShoppingListId { get; set; }
        public virtual List<ShoppingListDetail> ShoppingListDetails { get; set; }
        //public int BorgerForeignKey { get; set; }
    }

    public class ShoppingListDetail
    {
        public int ShoppingListDetailsId { get; set; }
        public int Quantity { get; set; }
        public bool Bought { get; set; }
        public Product Product { get; set; }
        public int ProductForeignKey { get; set; }
        public ShoppingList ShoppingList { get; set; }
        public int ShoppingListForeignKey { get; set; }
    }

    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public virtual ShoppingListDetail ShoppingListDetail { get; set; }
    }
}

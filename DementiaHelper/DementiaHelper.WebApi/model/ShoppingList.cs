using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DementiaHelper.WebApi.model
{
    public class ShoppingList
    {
        public int ShoppingListId { get; set; }
        [ForeignKey("RelativeConnection")]
        public RelativeConnection RelativeConnectionForeignKey { get; set; }
        [ForeignKey("CaregiverConnection")]
        public CaregiverConnection CaregiverConnectionForeignKey { get; set; }
    }

    public class ShoppingListDetail
    {
        public int ShoppingListDetailId { get; set; }
        public int Quantity { get; set; }
        public bool Bought { get; set; }
        [ForeignKey("Product")]
        public Product ProductForeignKey { get; set; }
        [ForeignKey("ShoppingList")]
        public ShoppingList ShoppingListForeignKey { get; set; }
    }

    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
    }
}

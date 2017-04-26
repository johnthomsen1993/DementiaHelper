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
        public int RelativeConnectionId { get; set; }
        public RelativeConnection RelativeConnection { get; set; }

        [ForeignKey("CaregiverConnection")]
        public int CaregiverConnectionId { get; set; }
        public CaregiverConnection CaregiverConnection { get; set; }
    }

    public class ShoppingListDetail
    {
        public int ShoppingListDetailId { get; set; }
        public int Quantity { get; set; }
        public bool Bought { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [ForeignKey("ShoppingList")]
        public int ShoppingListId { get; set; }
        public ShoppingList ShoppingList { get; set; }
    }

    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
    }
}

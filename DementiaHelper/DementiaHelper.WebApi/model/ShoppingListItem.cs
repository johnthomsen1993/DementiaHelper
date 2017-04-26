using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DementiaHelper.WebApi.model
{
    public class ShoppingListItem
    {
        public int ShoppingListItemId { get; set; }
        public int Quantity { get; set; }
        public bool Bought { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [ForeignKey("Citizen")]
        public int CitizenId { get; set; }
        public Citizen Citizen { get; set; }
    }
}

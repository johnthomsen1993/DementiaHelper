using DementiaHelper.Extensions;
using DementiaHelper.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DementiaHelper.ViewModel
{
    public class ShoppingListViewModel : BaseViewModel
    {
        public ShoppingList ShoppingList { get; set; }

        public ShoppingListViewModel()
        {
            ShoppingList = new ShoppingList() {ShoppingListDetails = new ObservableCollection<ShoppingListDetail>() {} };
            ShoppingList.ShoppingListDetails.Add(new ShoppingListDetail() { Product = new Product() { ProductName = "Test" }, Quantity = 1, Bought = false });
        }
    }
}

﻿using DementiaHelper.Extensions;
using DementiaHelper.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DementiaHelper.Services;
using Xamarin.Forms;

namespace DementiaHelper.PageModels
{
    public class ShoppingListPageModel : FreshMvvm.FreshBasePageModel
    {
        public const string URI_BASE = "http://dementiahelper.azurewebsites.net/api/values/shoppinglist";
        public const string URI_BASE_TEST = "http://localhost:29342/api/values/shoppinglist/";
        public ShoppingList ShoppingList { get; set; }
        public ICommand SaveToDatabaseCommand { get; protected set; }
        public string Item { get; set; }

        public ShoppingListPageModel()
        {
            ShoppingList = new ShoppingList() {ShoppingListDetails = new ObservableCollection<ShoppingListDetail>() {} };
            ShoppingList.ShoppingListDetails.Add(new ShoppingListDetail() { Product = new Product() { ProductName = "Test" }, Quantity = 1, Bought = false });

            this.SaveToDatabaseCommand = new Command(async () => await SaveToDatabase(Item));
        }

        async Task SaveToDatabase(string item)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var encoded = JWTService.Encode(new Dictionary<string, object>() { { "Item", item } });
                    StringContent content = new StringContent(encoded);
                    await client.PutAsync(new Uri(URI_BASE), content);

                }
                catch (Exception)
                {
                    
                }
            }
        }
    }
}

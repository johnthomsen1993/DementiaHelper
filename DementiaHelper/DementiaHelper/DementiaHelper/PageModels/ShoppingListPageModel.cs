﻿using DementiaHelper.Extensions;
using DementiaHelper.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DementiaHelper.Resx;
using DementiaHelper.Services;
using Newtonsoft.Json.Linq;
using PropertyChanged;
using Xamarin.Forms;

namespace DementiaHelper.PageModels
{
    [ImplementPropertyChanged]
    public class ShoppingListPageModel : FreshMvvm.FreshBasePageModel
    {
        public const string URI_BASE = "http://dementiahelper.azurewebsites.net/api/values/shoppinglist/";
        public const string URI_BASE_TEST = "http://localhost:29342/api/values/shoppinglist/";
        public ShoppingList ShoppingList { get; set; }
        public ICommand RemoveFromDatabaseCommand { get; protected set; }
        public ICommand CreateShoppingItemCommand { get; protected set; }
        public string Item { get; set; }
        public ObservableCollection<ShoppingListDetail> ShoppingListDetails { get; set; }

        public ShoppingListPageModel()
        {
            ShoppingList = new ShoppingList() {ShoppingListDetails = new ObservableCollection<ShoppingListDetail>() {} };
            var User = (ApplicationUser)App.Current.Properties["ApplicationUser"];
            Device.BeginInvokeOnMainThread(async () =>
            {
                var shoppinglist = await GetShoppingList(8); //TODO: Make citizenid accessible and change this
                ShoppingList.ShoppingListDetails = shoppinglist.ShoppingListDetails;
                ShoppingList.ShoppingListId = shoppinglist.ShoppingListId;
                ShoppingListDetails = ShoppingList.ShoppingListDetails;
            });
            CreateShoppingItemCommand = new Command(async () => await GoToCreateShoppingItem());
            RemoveFromDatabaseCommand = new Command(async (obj) => await RemoveFromDatabase((ShoppingListDetail) obj));
        }
        private async Task GoToCreateShoppingItem()
        {
            await CoreMethods.PushPageModel<CreateShoppingItemPageModel>();
        }
        private async Task RemoveFromDatabase(ShoppingListDetail item)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var encoded = JWTService.Encode(new Dictionary<string, object>() { { "shoppingListDetailId", item.ShoppingListDetailId }, {"citizenId", 8} }); //TODO: Make citizenid accessible and change this
                    var result = await client.DeleteAsync(new Uri(URI_BASE + encoded));
                    var test = await result.Content.ReadAsStringAsync();
                    var decoded = JWTService.Decode(test);
                    if (!decoded.ContainsKey("ErrorOnRemove"))
                    {
                        ShoppingList = MapToShoppingListModel(decoded);
                        ShoppingListDetails = ShoppingList.ShoppingListDetails;
                    }
                    else
                    {
                       await App.Current.MainPage.DisplayAlert(AppResources.ErrorOnRemoveTitle, AppResources.ErrorOnRemove, AppResources.ErrorOnRemoveAccept);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private async Task<ShoppingList> GetShoppingList(int? id)
        {
            if (id == null) { return null; }
            using (var client = new HttpClient())
            {
                try
                {
                    var encoded = JWTService.Encode(new Dictionary<string, object>() { { "citizenId", id } });
                    var result =  await client.GetStringAsync(new Uri(URI_BASE + encoded));
                    var decoded = JWTService.Decode(result);
                    return MapToShoppingListModel(decoded);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        private ShoppingList MapToShoppingListModel(IDictionary<string, object> dict)
        {
            ShoppingList tempShoppingList = new ShoppingList()
            {
                ShoppingListDetails = new ObservableCollection<ShoppingListDetail>()
            };
            var list = dict.Where(x => x.Key.Contains("ShoppingList")).Select(x => x.Value).ToList().FirstOrDefault() as IEnumerable<object>;
            foreach (var obj in list)
            {
                var jsonContainer = obj as JContainer;

                var shoppingListDetailId = jsonContainer.SelectToken("ShoppingListDetailId");
                var bought = jsonContainer.SelectToken("Bought");
                var quantity = jsonContainer.SelectToken("Quantity");

                //Product
                var jsonProduct = jsonContainer.SelectToken("ProductForeignKey");
                var productName = jsonProduct.SelectToken("ProductName");
                var productId = jsonProduct.SelectToken("ProductId");

                //ShoppingList
                var jsonShoppingList = jsonContainer.SelectToken("ShoppingListForeignKey");
                var shoppingListId = jsonShoppingList.SelectToken("ShoppingListId");

                tempShoppingList.ShoppingListId = shoppingListId.ToObject<int>();
                tempShoppingList.ShoppingListDetails.Add(new ShoppingListDetail()
                {
                    Bought = bought.ToObject<bool>(),
                    Quantity = quantity.ToObject<int>(),
                    ShoppingListDetailId = shoppingListDetailId.ToObject<int>(),
                    Product = new Product() {
                        ProductName = productName.ToString(),
                        ProductId = productId.ToObject<int>()}
                });
            }
            return tempShoppingList;
        }
    }
}

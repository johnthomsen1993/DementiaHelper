using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DementiaHelper.Resx;
using DementiaHelper.Services;
using Newtonsoft.Json.Linq;

namespace DementiaHelper.Model.Controllers
{
    public class ShoppingListController : IShoppingListController
    {
        public async Task AddShoppingItemToShoppingList(int citizenId, string item, int quantity)
        {

                using (var client = new HttpClient())
                {

                    var encoded = JWTService.Encode(new Dictionary<string, object>() { { "CitizenId", citizenId }, { "Item", item }, { "Quantity", quantity } });
                    var values = new Dictionary<string, string> { { "token", encoded } };
                    var content = new FormUrlEncodedContent(values);
                    var result = await client.PutAsync(new Uri("http://dementiahelper.azurewebsites.net/api/values/shoppinglist/"), content);
                    JWTService.Decode(await result.Content.ReadAsStringAsync());
                   
                }
        }
        public async Task<IDictionary<string, object>> ChangeBoughtStateOfItem(ShoppingListItem item)
        {

            using (var client = new HttpClient())
            {
                try
                {
                    var encoded = JWTService.Encode(new Dictionary<string, object>() { { "ShoppingListItemId", item.ShoppingListItemId }, { "Bought", item.Bought }, { "CitizenId", item.CitizenId } });
                    var values = new Dictionary<string, string> { { "token", encoded } };
                    var content = new FormUrlEncodedContent(values);
                    var result = await client.PutAsync(new Uri("http://dementiahelper.azurewebsites.net/api/values/shoppinglist/bought/"), content);
                    return JWTService.Decode(await result.Content.ReadAsStringAsync());
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public async Task<IDictionary<string, object>> RemoveShoppingItem(ShoppingListItem item)
        {
            using (var client = new HttpClient())
            {
                    var encoded = JWTService.Encode(new Dictionary<string, object>() { { "shoppingListItemId", item.ShoppingListItemId }, { "citizenId", ((ApplicationUser)App.Current.Properties["ApplicationUser"]).CitizenId } });
                    var result = await client.DeleteAsync(new Uri("http://dementiahelper.azurewebsites.net/api/values/shoppinglist/" + encoded));
                    return JWTService.Decode(await result.Content.ReadAsStringAsync());
            }
        }


        public async Task<ShoppingList> GetShoppingList(int? id)
        {
            if (id == null) { return null; }
            using (var client = new HttpClient())
            {
                try
                {
                    var encoded = JWTService.Encode(new Dictionary<string, object>() { { "citizenId", id } });
                    var result = await client.GetStringAsync(new Uri("http://dementiahelper.azurewebsites.net/api/values/shoppinglist/" + encoded));
                    var decoded = JWTService.Decode(result);
                    return decoded.ContainsKey("List") ? null : MapToShoppingListModel(decoded);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }







        public ShoppingList MapToShoppingListModel(IDictionary<string, object> dict)
        {
            ShoppingList tempShoppingList = new ShoppingList()
            {
                ShoppingListItems = new ObservableCollection<ShoppingListItem>()
            };
            var list = dict.SingleOrDefault(x => x.Key.Equals("ShoppingList")).Value as IEnumerable<object>;
            foreach (var obj in list)
            {
                var jsonContainer = obj as JContainer;

                var shoppingListItemId = jsonContainer.SelectToken("ShoppingListItemId");
                var bought = jsonContainer.SelectToken("Bought");
                var quantity = jsonContainer.SelectToken("Quantity");

                //Product
                var jsonProduct = jsonContainer.SelectToken("Product");
                var productName = jsonProduct.SelectToken("ProductName");
                var productId = jsonProduct.SelectToken("ProductId");

                //CitizenId
                var jsonShoppingList = jsonContainer.SelectToken("CitizenId");

                tempShoppingList.ShoppingListItems.Add(new ShoppingListItem()
                {
                    Bought = bought.ToObject<bool>(),
                    Quantity = quantity.ToObject<int>(),
                    ShoppingListItemId = shoppingListItemId.ToObject<int>(),
                    CitizenId = jsonShoppingList.ToObject<int>(),
                    Product = new Product()
                    {
                        ProductName = productName.ToString(),
                        ProductId = productId.ToObject<int>()
                    }
                });
            }
            return tempShoppingList;
        }
    }
}
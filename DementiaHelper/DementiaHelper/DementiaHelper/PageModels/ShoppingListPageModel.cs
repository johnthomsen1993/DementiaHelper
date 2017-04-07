using DementiaHelper.Extensions;
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
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace DementiaHelper.PageModels
{
    public class ShoppingListPageModel : FreshMvvm.FreshBasePageModel
    {
        public const string URI_BASE = "http://dementiahelper.azurewebsites.net/api/values/shoppinglist/";
        public const string URI_BASE_TEST = "http://localhost:29342/api/values/shoppinglist/";
        public ICommand UpdateCommand { get; protected set; }
        public ShoppingList ShoppingList { get; set; }
        public ICommand SaveToDatabaseCommand { get; protected set; }
        public string Item { get; set; }

        public ShoppingListPageModel()
        {
            this.UpdateCommand = new Command(async () => await GetShoppingList(8));
            ShoppingList = new ShoppingList() {ShoppingListDetails = new ObservableCollection<ShoppingListDetail>() {} };
            Device.BeginInvokeOnMainThread(async () =>
            {
                var shoppinglist = await GetShoppingList(8);
                ShoppingList.ShoppingListDetails = shoppinglist.ShoppingListDetails;
            });
        }

        

        private async Task<ShoppingList> GetShoppingList(int id)
        {
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
            var smth = dict.Where(x => x.Key.Contains("ShoppingList")).Select(x => x.Value).ToList();
            var okay = smth.FirstOrDefault() as IEnumerable<object>;
            foreach (var obj in okay)
            {
                var test = obj as JContainer;
                var hello = test.SelectToken("ProductForeignKey");
                var mmm = hello.SelectToken("ProductName");
                hello = hello;
            }
            //tempShoppingList.ShoppingListDetails.Add(new ShoppingListDetail()
            //{
            //    Bought = ,
            //    Quantity = o.Key == "Quantity" ? o.Value as int? : null,
            //    ShoppingListDetailId = o.Key == "ShoppingListDetailId" ? o.Value as int? : null,
            //    Product = new Product()
            //    {
            //        ProductId = o.Key == "ProductId" ? o.Value as int? : null,
            //        ProductName = o.Key == "ProductName" ? o.Value as string : null
            //    }
            //});
            return null;
        }
    }
}

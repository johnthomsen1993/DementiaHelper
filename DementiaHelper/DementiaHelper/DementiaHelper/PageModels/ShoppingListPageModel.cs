using DementiaHelper.Extensions;
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
        public ShoppingList ShoppingList { get; set; }
        public ICommand RemoveFromDatabaseCommand { get; protected set; }
        public ICommand CreateShoppingItemCommand { get; protected set; }
        public string Item { get; set; }
        public ObservableCollection<ShoppingListItem> ShoppingListDetails { get; set; }
        ApplicationUser User = (ApplicationUser)App.Current.Properties["ApplicationUser"];
        public ShoppingListPageModel()
        {
            ShoppingList = new ShoppingList() {ShoppingListItems = new ObservableCollection<ShoppingListItem>() {} };
            CreateShoppingItemCommand = new Command(async (id) => await GoToCreateShoppingItem(User.CitizenId));
            RemoveFromDatabaseCommand = new Command(async (obj) => await RemoveFromDatabase((ShoppingListItem) obj));
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            var User = (ApplicationUser)App.Current.Properties["ApplicationUser"];
            Device.BeginInvokeOnMainThread(async () =>
            {
                var shoppinglist = await GetShoppingList(User.CitizenId);
                ShoppingList.ShoppingListItems = shoppinglist?.ShoppingListItems;
                ShoppingListDetails = ShoppingList?.ShoppingListItems;
            });
        }

        private async Task GoToCreateShoppingItem(int? id)
        {
            if (id != null)
            {
                await CoreMethods.PushPageModel<CreateShoppingItemPageModel>(id);
            }
            else
            {
                if (User.RoleId == 2) { await CoreMethods.DisplayAlert("Not possible", "to add new items, you need to be connected to a person under care", "Ok"); }
                if (User.RoleId == 3) { await CoreMethods.DisplayAlert("Not possible", "to add new items, you need to have choosen the citizen your inspecting", "Ok"); }
            }
        }
        private async Task RemoveFromDatabase(ShoppingListItem item)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var encoded = JWTService.Encode(new Dictionary<string, object>() { { "shoppingListItemId", item.ShoppingListItemId }, {"citizenId", 8} }); //TODO: Make citizenid accessible and change this
                    var result = await client.DeleteAsync(new Uri(URI_BASE + encoded));
                    var test = await result.Content.ReadAsStringAsync();
                    var decoded = JWTService.Decode(test);
                    if (!decoded.ContainsKey("ErrorOnRemove"))
                    {
                        ShoppingList = MapToShoppingListModel(decoded);
                        ShoppingListDetails = ShoppingList.ShoppingListItems;
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
                    return decoded.ContainsKey("List") ? null : MapToShoppingListModel(decoded);
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
                ShoppingListItems = new ObservableCollection<ShoppingListItem>()
            };
            var list = dict.SingleOrDefault(x => x.Key.Equals("ShoppingList")).Value as IEnumerable<object>;
            //var list = dict.Where(x => x.Key.Contains("ShoppingList")).Select(x => x.Value).ToList().FirstOrDefault() as IEnumerable<object>;
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
                    Product = new Product() {
                        ProductName = productName.ToString(),
                        ProductId = productId.ToObject<int>()}
                });
            }
            return tempShoppingList;
        }
    }
}

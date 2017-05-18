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
        public ShoppingList ShoppingList { get; set; }
        public ICommand RemoveShoppingItemCommand { get; protected set; }
        public ICommand ChangeBoughtStateOfItemCommand { get; protected set; }
        public ICommand GoToCreateShoppingItemCommand { get; protected set; }
        public string Item { get; set; }
        public ObservableCollection<ShoppingListItem> ShoppingListDetails { get; set; }
        public ShoppingListPageModel()
        {
            ShoppingList = new ShoppingList() {ShoppingListItems = new ObservableCollection<ShoppingListItem>() {} };
            GoToCreateShoppingItemCommand = new Command(async (id) => await GoToCreateShoppingItem(((ApplicationUser)App.Current.Properties["ApplicationUser"]).CitizenId));
            RemoveShoppingItemCommand = new Command(async (obj) => await RemoveShoppingItem((ShoppingListItem) obj));
            ChangeBoughtStateOfItemCommand = new Command(async (obj) => await ChangeBoughtStateOfItem((ShoppingListItem)obj));
        }

        private async Task ChangeBoughtStateOfItem(ShoppingListItem item)
        {
            var decoded = await ModelAccessor.Instance.ShoppingListController.ChangeBoughtStateOfItem(item);
            if (!decoded.ContainsKey("ErrorOnUpdate"))
            {
                ShoppingList = ModelAccessor.Instance.ShoppingListController.MapToShoppingListModel(decoded);
                ShoppingListDetails = ShoppingList.ShoppingListItems;
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(AppResources.ErrorOnRemoveTitle, AppResources.ErrorOnRemove, AppResources.General_Ok);
            }
        }


        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            var User = (ApplicationUser)App.Current.Properties["ApplicationUser"];
            Device.BeginInvokeOnMainThread(async () =>
            {
                var shoppinglist = await ModelAccessor.Instance.ShoppingListController.GetShoppingList(User.CitizenId);
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
                switch (((ApplicationUser) App.Current.Properties["ApplicationUser"]).RoleId)
                {
                    case 2:
                        await CoreMethods.DisplayAlert("Not possible",
                            "to add new items, you need to be connected to a person under care", "Ok");
                        break;
                    case 3:
                        await CoreMethods.DisplayAlert("Not possible",
                            "to add new items, you need to have choosen the citizen your inspecting", "Ok");
                        break;
                }
            }
        }
        private async Task RemoveShoppingItem(ShoppingListItem item)
        {
            var decoded = await ModelAccessor.Instance.ShoppingListController.RemoveShoppingItem(item);
            if (!decoded.ContainsKey("ErrorOnRemove"))
            {
                ShoppingList = ModelAccessor.Instance.ShoppingListController.MapToShoppingListModel(decoded);
                ShoppingListDetails = ShoppingList.ShoppingListItems;
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(AppResources.ErrorOnRemoveTitle, AppResources.ErrorOnRemove, AppResources.General_Ok);
            }
        }
    }
}

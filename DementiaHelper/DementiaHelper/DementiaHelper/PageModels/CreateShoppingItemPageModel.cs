using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DementiaHelper.Model;
using DementiaHelper.Services;
using Xamarin.Forms;

namespace DementiaHelper.PageModels
{
    class CreateShoppingItemPageModel : FreshMvvm.FreshBasePageModel
    {
        #region ViewModel Properties
        public ICommand AddShoppingItemToShoppingListCommand { get; protected set; }
        public ICommand CancelAddShoppingItemToShoppingListCommand { get; protected set; }
        public string Item { get; set; }
        public int CitizenId { get; set; }
        public int Quantity { get; set; }
        #endregion

        public CreateShoppingItemPageModel ()
        {
            Quantity = 1;
            AddShoppingItemToShoppingListCommand = new Command(async () => await AddShoppingItemToShoppingList());
            CancelAddShoppingItemToShoppingListCommand = new Command(async () => await Cancel());
        }

        public override void Init(object initData)
        {
            base.Init(initData);
            CitizenId = (int) initData;
        }

        async Task AddShoppingItemToShoppingList()
        {
            await ModelAccessor.Instance.ShoppingListController.AddShoppingItemToShoppingList(CitizenId, Item, Quantity);
            await CoreMethods.PopPageModel();
        }

        async Task Cancel()
        {
            await CoreMethods.PopPageModel();
        }
    }
}

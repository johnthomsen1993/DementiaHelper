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
    class AddItem : FreshMvvm.FreshBasePageModel
    {
        public const string URI_BASE = "http://dementiahelper.azurewebsites.net/api/values/shoppinglist";
        public const string URI_BASE_TEST = "http://localhost:29342/api/values/shoppinglist/";
        public ICommand SaveToDatabaseCommand { get; protected set; }
        public ICommand CancelCommand { get; protected set; }
        public string Item { get; set; }
        public ShoppingList ShoppingList { get; set; }
        public int Quantity { get; set; }


        public AddItem (ShoppingList list)
        {
            ShoppingList = list;
            SaveToDatabaseCommand = new Command(async () => await SaveToDatabase());
            CancelCommand = new Command(async () => await Cancel());
        }

        async Task SaveToDatabase()
        {
            using (var client = new HttpClient())
            { 
                try
                {
                    var encoded = JWTService.Encode(new Dictionary<string, object>() { {"ShoppingListId", ShoppingList}, { "Item", Item }, { "Quantity", Quantity } });
                    StringContent content = new StringContent(encoded);
                    await client.PutAsync(new Uri(URI_BASE), content);
                }
                catch (Exception)
                {
                }
            }
            await CoreMethods.PopPageModel();
        }

        async Task Cancel()
        {
            await CoreMethods.PopPageModel();
        }
    }
}

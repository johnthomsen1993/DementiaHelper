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
        public const string URI_BASE = "http://dementiahelper.azurewebsites.net/api/values/shoppinglist/";
        public const string URI_BASE_TEST = "http://localhost:29342/api/values/shoppinglist/";
        public ICommand SaveCommand { get; protected set; }
        public ICommand CancelCommand { get; protected set; }
        public string Item { get; set; }
        public int CitizenId { get; set; }
        public int Quantity { get; set; }


        public CreateShoppingItemPageModel ()
        {
            Quantity = 1;
            SaveCommand = new Command(async () => await SaveToDatabase());
            CancelCommand = new Command(async () => await Cancel());
        }

        public override void Init(object initData)
        {
            base.Init(initData);
            CitizenId = (int) initData;
        }

        async Task SaveToDatabase()
        {
            using (var client = new HttpClient())
            { 
                try
                {
                    var encoded = JWTService.Encode(new Dictionary<string, object>() { {"CitizenId", CitizenId }, { "Item", Item }, { "Quantity", Quantity } });
                    var values = new Dictionary<string, string> { { "content", encoded } };
                    var content = new FormUrlEncodedContent(values);
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

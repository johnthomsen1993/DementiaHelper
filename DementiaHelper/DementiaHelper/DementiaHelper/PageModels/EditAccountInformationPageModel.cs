using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DementiaHelper.Extensions;
using DementiaHelper.Model;
using DementiaHelper.Services;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Agreement.JPake;
using Xamarin.Forms;

namespace DementiaHelper.PageModels
{
    class EditAccountInformationPageModel : FreshMvvm.FreshBasePageModel
    {
        public const string URI_BASE = "http://dementiahelper.azurewebsites.net/api/values/save/";
        public const string URI_BASE_TEST = "http://localhost:29342/api/values/save/";
        public UserInformation User { get; set; }
        public ICommand SaveCommand { get; protected set; }
        public ICommand CancelCommand { get; protected set; }

        public EditAccountInformationPageModel(UserInformation user)
        {
            User = user;
            this.SaveCommand = new Command(async () => await Save());
            this.CancelCommand = new Command(async () => await Cancel());
        }
        async Task Save()
        {
            var values = new Dictionary<string, object>
            {
                {"FirstName", User.FirstName},
                {"LastName", User.LastName},
                {"Email", User.Email},
                {"Description", User.Description}
            };

            var payload = JWTService.Encode(values);

            using (HttpClient h = new HttpClient())
            {
                var content = new StringContent(payload);
                var result = h.PostAsync(new Uri(URI_BASE), content).Result;
                var response = result.Content.ReadAsStringAsync();
                await App.Current.MainPage.DisplayAlert(response.Result, "Test", "OK");
            }

            await CoreMethods.PopPageModel();
        }
        async Task Cancel()
        {
            await CoreMethods.PopPageModel();
        }

        
    }
}

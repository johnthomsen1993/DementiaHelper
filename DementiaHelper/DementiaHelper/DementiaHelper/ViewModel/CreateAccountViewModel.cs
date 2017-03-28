using System;
using System.Threading.Tasks;
using System.Windows.Input;
using DementiaHelper.Extensions;
using DementiaHelper.Model;
using DementiaHelper.Services;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Net.Http;

namespace DementiaHelper.ViewModel
{
    public class CreateAccountViewModel : BaseViewModel
    {
        public UserInformation User { get; set; }
        public string Password { get; set; }
        public ICommand CancelCreateAccountCommand { get; protected set; }
        public ICommand CreateAccountCommand { get; protected set; }
        public CreateAccountViewModel()
        {
            User = new UserInformation();
            this.CancelCreateAccountCommand = new Command(async () => await NavigationService.PopModalAsync());
            this.CreateAccountCommand = new Command( async() => await CreateAccountAsync());
        }

        async Task CreateAccountAsync()
        {
            var values = new Dictionary<string, string>
            {
                {"email",User.Email},
                {"password", Password}
            };
            using (var h = new HttpClient())
            {
                var content = new FormUrlEncodedContent(values);
                var result = h.PostAsync(new Uri("http://dementiahelper.azurewebsites.net/api/account/createaccount"), content).Result;
                var response = result.Content.ReadAsStringAsync();
                await App.Current.MainPage.DisplayAlert(response.Result, "Test", "OK");
            }
        }

    }
}
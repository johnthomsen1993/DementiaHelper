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
        public string Email { get; set; }
        public string Password { get; set; }
        public ICommand CancelCreateAccountCommand { get; protected set; }
        public ICommand CreateAccountCommand { get; protected set; }
        public CreateAccountViewModel()
        {
            HttpClient h = new HttpClient();
        
            this.CancelCreateAccountCommand = new Command(async () => await NavigationService.PopModalAsync());
            this.CreateAccountCommand = new Command( ()=>
            {
                //  App.Current.MainPage.DisplayAlert(User.Name, "Test", "OK");
                // DependencyService.Get<ICredentialsService>().SaveCredentials(Email,Password);
                var body = new List<KeyValuePair<string, string>>
          {
                 new KeyValuePair<string, string>("userName", Email),
                 new KeyValuePair<string, string>("password", Password),
          };

                var content = new FormUrlEncodedContent(body);
                var result =  h.PostAsync(new Uri("http://dementiahelperwebapi20170303124653.azurewebsites.net/api/values/register"), content).Result;
                var response =  result.Content.ReadAsStringAsync();
                App.Current.MainPage.DisplayAlert(response.Result, "Test", "OK");
            });
         
        }

    }
}
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
        public User User { get; set; }
        public ICommand CancelCreateAccountCommand { get; protected set; }
        public ICommand CreateAccountCommand { get; protected set; }
        public CreateAccountViewModel()
        {
            HttpClient h = new HttpClient();
            User = new User();
            this.CancelCreateAccountCommand = new Command(async () => await NavigationService.PopModalAsync());
            this.CreateAccountCommand = new Command<User>( (User) =>
            {
             //  App.Current.MainPage.DisplayAlert(User.Name, "Test", "OK");
              // DependencyService.Get<ICredentialsService>().SaveCredentials(User.Name,"password");
                var body = new List<KeyValuePair<string, string>>
          {
                 new KeyValuePair<string, string>("userName", "arg1value"),
                 new KeyValuePair<string, string>("password", "arg2value"),
          };

                var content = new FormUrlEncodedContent(body);
                var result =  h.PostAsync(new Uri("http://dementiahelperwebapi20170302110209.azurewebsites.net/api/values/register"), content).Result;
                var response =  result.Content.ReadAsStringAsync();
                App.Current.MainPage.DisplayAlert(response.Result, "Test", "OK");
            });
         
        }

    }
}
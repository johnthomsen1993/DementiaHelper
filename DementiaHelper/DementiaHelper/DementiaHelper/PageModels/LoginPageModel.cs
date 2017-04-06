using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DementiaHelper.Extensions;
using System.Windows.Input;
using DementiaHelper.Services;
using Xamarin.Forms;
using DementiaHelper.Pages;

namespace DementiaHelper.PageModels
{
   
   public class LoginPageModel : FreshMvvm.FreshBasePageModel
    {
      public string UserName { get; set; }
      public string Password { get; set; }
      public ICommand LoginCommand { get; protected set; }
      public ICommand GoToCreateAccountCommand { get; protected set; }
      public ICommand GoToAccountInformationCommand { get; protected set; }

        public LoginPageModel()
        {
        

        }
        public override void Init(object initData)
        {
            base.Init(initData);
            this.LoginCommand = new Command( () =>  LoginAsync());
            this.GoToCreateAccountCommand = new Command(async () => await GoToCreateAccount());
            this.GoToAccountInformationCommand = new Command(async () => await GoToAccountInformation());
        }

         void LoginAsync()
        {
            var values = new Dictionary<string, string>
            {
                {"userName",UserName},
                {"password", Password}
            };
            CoreMethods.SwitchOutRootNavigation(App.NavigationStacks.MainAppStack);
           /* using (var h = new HttpClient()){
                var content = new FormUrlEncodedContent(values);
                var result = h.PostAsync(new Uri("http://dementiahelper.azurewebsites.net/api/account/"), content).Result;
                var response = result.Content.ReadAsStringAsync();
                await App.Current.MainPage.DisplayAlert(response.Result, "Test", "OK");
               // await NavigationService.PushAsync(new ClockViewModel());
            }*/
        }
        async Task GoToCreateAccount()
        {
            
            await CoreMethods.PushPageModel<CreateAccountPageModel>();
        }
        async Task GoToAccountInformation()
        {
            
            await CoreMethods.PushPageModel<AccountInformationPageModel>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using DementiaHelper.Services;
using System.Text;
using System.Threading.Tasks;
using DementiaHelper.Model;
using Xamarin.Forms;
using DementiaHelper.ViewModel;
using DementiaHelper.View;

namespace DementiaHelper
{
    public interface IAuthenticate
    {
        Task<bool> Authenticate();
    }
    public partial class App : Application
    {
        public static string AppName { get { return "Dementia Helper"; } }
        public static string UserName { get { return "John"; } }
        public static string Password { get { return "passwords"; } }
        public App()
        {
            RegisterPages();
            if (DependencyService.Get<ICredentialsService>().Authenticate()){
                NavigationService.SetRoot(new ClockViewModel());
            } else {
                NavigationService.SetRoot(new LoginPageViewModel());
            }
        }

        void RegisterPages()
        {
            SimpleIoC.RegisterPage<LoginPageViewModel, LoginPage>();
            SimpleIoC.RegisterPage<ClockViewModel, MainPage>();
            SimpleIoC.RegisterPage<CreateAccountViewModel, CreateAccount>();
            SimpleIoC.RegisterPage<AccountInformationViewModel, AccountInformation>();
            SimpleIoC.RegisterPage<EditAccountInformationViewModel, EditAccountInformation>();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

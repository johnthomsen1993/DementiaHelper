using System;
using System.Collections.Generic;
using System.Linq;
using DementiaHelper.Services;
using System.Text;
using System.Threading.Tasks;

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
        public App()
        {
            RegisterPages();
            NavigationService.SetRoot(new LoginPageViewModel());
        }

        void RegisterPages()
        {
            SimpleIoC.RegisterPage<LoginPageViewModel, LoginPage>();
            SimpleIoC.RegisterPage<ClockViewModel, MainPage>();
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

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
            InitializeComponent();
            if (Device.OS == TargetPlatform.iOS || Device.OS == TargetPlatform.Android)
            {
                var ci = DependencyService.Get<Resx.ILocalize>().GetCurrentCultureInfo();
                Resx.AppResources.Culture = ci; // set the RESX for resource localization
                DependencyService.Get<Resx.ILocalize>().SetLocale(ci); // set the Thread for locale-aware methods
            }
            var tabs = new TabbedPage();
            tabs.Children.Add(new View.ContactList { Title = Resx.AppResources.ContactList});
            tabs.Children.Add(new View.MainPage { Title = Resx.AppResources.ShoppingList});

            MainPage = tabs;
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

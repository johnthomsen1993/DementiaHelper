using System;
using System.Collections.Generic;
using System.Linq;
using DementiaHelper.Services;
using System.Text;
using System.Threading.Tasks;
using DementiaHelper.Model;
using Xamarin.Forms;

using DementiaHelper.PageModels;
using DementiaHelper.Pages;
using DementiaHelper.Resx;

namespace DementiaHelper
{
    public interface IAuthenticate
    {
        Task<bool> Authenticate();
    }
    public partial class App : Application
    {
        //Setup names for you navigation stacks
        public class NavigationStacks
        {
            public static string LoginNavigationStack = "LoginNavigationStack";
            public static string MainAppStack = "MainAppStack";
        }
        public static string AppName { get { return "Dementia Helper"; } }
        public static string UserName { get { return "John"; } }
        public static string Password { get { return "password"; } }
        public App()
        {
            InitializeComponent();
            var masterDetailNav = new FreshMvvm.FreshMasterDetailNavigationContainer(NavigationStacks.MainAppStack);
            masterDetailNav.Init("menu");
            masterDetailNav.AddPage<ContactListPageModel>(AppResources.ContactListTitle, null);
            masterDetailNav.AddPage<ImageGalleryPageModel>("Foto minder", null);
            masterDetailNav.AddPage<ChatPageModel>("Chat", null);
            masterDetailNav.AddPage<CalenderPageModel>("Kalender", null);
            masterDetailNav.AddPage<ChooseCitizenPageModel>("Choose Citizen", null);
            masterDetailNav.AddPage<ShoppingListPageModel>("Shopping list", null);
            masterDetailNav.AddPage<ConnectToCitizenPageModel>(AppResources.ConnectToCitizenTitle, null);
            masterDetailNav.AddPage<AccountInformationPageModel>("Account Information", null);
      //      masterDetailNav.AddPage<CitizenHomePageModel>("Idag", null);
            
            var Login = FreshMvvm.FreshPageModelResolver.ResolvePageModel<LoginPageModel>();
            var navContainer = new FreshMvvm.FreshNavigationContainer(Login, NavigationStacks.LoginNavigationStack);
           
            DependencyService.Get<ICredentialsService>().SaveCredentials(UserName, Password+"s");
            if (DependencyService.Get<ICredentialsService>().Authenticate()){
                      MainPage = masterDetailNav;
            }
            else {
                MainPage = navContainer;
            }
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

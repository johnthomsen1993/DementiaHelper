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
        public string ApplicationUser { get; set; }
        public static string Password { get { return "password"; } }
        static FreshMvvm.FreshMasterDetailNavigationContainer masterDetailNav = new FreshMvvm.FreshMasterDetailNavigationContainer(NavigationStacks.MainAppStack);
        public App()
        {
            InitializeComponent();
            masterDetailNav.Init("Menu");


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

       static public void SetMasterDetailToRole()
        {
            var h =App.Current.Properties["Test"];
           
            switch ((string)App.Current.Properties["ApplicationUser"])
            {
                case "Relatives":
                    {
                        masterDetailNav.AddPage<ConnectToCitizenPageModel>(AppResources.ConnectToCitizenTitle, null);
                        masterDetailNav.AddPage<ImageGalleryPageModel>(AppResources.ImageGalleryTitle, null);
                        masterDetailNav.AddPage<ShoppingListPageModel>(AppResources.ShoppingListTitle, null);
                        masterDetailNav.AddPage<ChatPageModel>(AppResources.ChatTitle, null);
                        masterDetailNav.AddPage<CalenderPageModel>(AppResources.CalenderTitle, null);
                        masterDetailNav.AddPage<AccountInformationPageModel>(AppResources.AccountInformationTitle, null);
                        break;
                    }
                case "Citizen":
                    {
                        masterDetailNav.AddPage<CitizenHomePageModel>("Idag", null);
                        masterDetailNav.AddPage<ImageGalleryPageModel>(AppResources.ImageGalleryTitle, null);
                        masterDetailNav.AddPage<ShoppingListPageModel>(AppResources.ShoppingListTitle, null);
                        masterDetailNav.AddPage<ChatPageModel>(AppResources.ChatTitle, null);
                        masterDetailNav.AddPage<CalenderPageModel>(AppResources.CalenderTitle, null);
                        masterDetailNav.AddPage<AccountInformationPageModel>(AppResources.AccountInformationTitle, null);
                        break;
                    }
                case "Caregiver":
                    {
                        masterDetailNav.AddPage<ChooseCitizenPageModel>(AppResources.ChooseCitizenTitle, null);
                        masterDetailNav.AddPage<ShoppingListPageModel>(AppResources.ShoppingListTitle, null);
                        masterDetailNav.AddPage<ChatPageModel>(AppResources.ChatTitle, null);
                        masterDetailNav.AddPage<CalenderPageModel>(AppResources.CalenderTitle, null);
                        masterDetailNav.AddPage<AccountInformationPageModel>(AppResources.AccountInformationTitle, null);
                        break;
                    }

                default:
                    {
                        break;
                    }

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

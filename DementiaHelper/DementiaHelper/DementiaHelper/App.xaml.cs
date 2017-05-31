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
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Collections.ObjectModel;
using System.Collections;

namespace DementiaHelper
{
    public interface IAuthenticate
    {
        Task<bool> Authenticate();
    }
    public partial class App : Application
    {
        /// <summary>
        /// Found out how to make different navigation stacks at https://github.com/rid00z/FreshMvvm
        /// </summary>
        public class NavigationStacks
        {
            public static string LoginNavigationStack = "LoginNavigationStack";
            public static string MainAppStack = "MainAppStack";
        }
        public static string AppName { get { return "Cura Civis"; } }
        public ApplicationUser ApplicationUser { get; set; }
        static FreshMvvm.CustomMasterDetailNavigation MasterDetailNav { get; set; }
        static FreshMvvm.FreshNavigationContainer LoginNavigationContainer { get; set; }
        public App()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Adapted ways to setup the FreshNavigationContainer from https://github.com/rid00z/FreshMvvm
        /// </summary>
        static public void SetLoginPageContainer()
        {
            var Login = FreshMvvm.FreshPageModelResolver.ResolvePageModel<LoginPageModel>();
            LoginNavigationContainer = new FreshMvvm.FreshNavigationContainer(Login, NavigationStacks.LoginNavigationStack);
        }

        /// <summary>
        /// Adapted ways to setup the FreshMasterDetail from https://github.com/rid00z/FreshMvvm
        /// </summary>
        static public void SetMasterDetailToRole()
        {

            var User = (ApplicationUser)App.Current.Properties["ApplicationUser"];
            switch (User.RoleId)
            {
                case 1:
                    { 
                        MasterDetailNav = new FreshMvvm.CustomMasterDetailNavigation(NavigationStacks.MainAppStack);
                        MasterDetailNav.Init("Menu");
                        MasterDetailNav.AddPage<CitizenHomePageModel>(AppResources.CitizenHomeTitle, null);
                        MasterDetailNav.AddPage<ContactListPageModel>(AppResources.ContactListTitle, null);
                       // MasterDetailNav.AddPage<ImageGalleryPageModel>(AppResources.ImageGalleryTitle, null);
                        MasterDetailNav.AddPage<ShoppingListPageModel>(AppResources.ShoppingListTitle, null);
                        MasterDetailNav.AddPage<ChatPageModel>(AppResources.ChatTitle, 1);
                        MasterDetailNav.AddPage<CalendarPageModel>(AppResources.CalenderTitle, null);
                        MasterDetailNav.AddPage<AccountInformationPageModel>(AppResources.AccountInformationTitle, null);
                        MasterDetailNav.AddPage<ConnectToNursingHomePageModel>(AppResources.ConnectToNursingHomeTitle, null);
                        MasterDetailNav.AddPage<ChoosePrimaryRelativePageModel>(AppResources.ChoosePrimaryRelative, null);
                        MasterDetailNav.AddPage<LogOutPageModel>(AppResources.LogOutTitle, null);
                        break;
                    }
                case 2:
                    {
                        
                        MasterDetailNav = new FreshMvvm.CustomMasterDetailNavigation(NavigationStacks.MainAppStack);
                        MasterDetailNav.Init("Menu");
                        MasterDetailNav.AddPage<ConnectToCitizenPageModel>(AppResources.ConnectToCitizenTitle, null);
                        MasterDetailNav.AddPage<ContactListPageModel>(AppResources.ContactListTitle, null);
                        //MasterDetailNav.AddPage<ImageGalleryPageModel>(AppResources.ImageGalleryTitle, null);
                        MasterDetailNav.AddPage<ShoppingListPageModel>(AppResources.ShoppingListTitle, null);
                        MasterDetailNav.AddPage<ChatPageModel>(AppResources.Chat_ChatTitleRelative, 2);
                        if (User.PrimaryRelative)
                        {
                            MasterDetailNav.AddPage<ChatPageModel>(AppResources.Chat_ChatTitleCitizen, 1);
                            MasterDetailNav.AddPage<ChatPageModel>(AppResources.Chat_ChatTitleCaretaker, 3);
                        }
                        MasterDetailNav.AddPage<CalendarPageModel>(AppResources.CalenderTitle, null);
                        MasterDetailNav.AddPage<AccountInformationPageModel>(AppResources.AccountInformationTitle, null);
                        MasterDetailNav.AddPage<LogOutPageModel>(AppResources.LogOutTitle, null);
                        break;
                    }
                case 3:
                    {
                        MasterDetailNav = new FreshMvvm.CustomMasterDetailNavigation(NavigationStacks.MainAppStack);
                        MasterDetailNav.Init("Menu");
                        MasterDetailNav.AddPage<ChooseCitizenPageModel>(AppResources.ChooseCitizenTitle, null);
                        MasterDetailNav.AddPage<ContactListPageModel>(AppResources.ContactListTitle, null);
                        MasterDetailNav.AddPage<ShoppingListPageModel>(AppResources.ShoppingListTitle, null);
                        MasterDetailNav.AddPage<ChatPageModel>(AppResources.ChatTitle, 3);
                        MasterDetailNav.AddPage<CalendarPageModel>(AppResources.CalenderTitle, null);
                        MasterDetailNav.AddPage<AccountInformationPageModel>(AppResources.AccountInformationTitle, null);
                        MasterDetailNav.AddPage<NotePageModel>(AppResources.NoteTitle, null);
                        MasterDetailNav.AddPage<LogOutPageModel>(AppResources.LogOutTitle, null);
                        break;
                    }
            }
                
        }

        
        protected override void OnStart()
        {
            if(DependencyService.Get<ICredentialsService>().Authenticate()){
                    App.SetMasterDetailToRole();
                    MainPage = MasterDetailNav;
            }
            else
            {
                SetLoginPageContainer();
                MainPage = LoginNavigationContainer;

            }
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

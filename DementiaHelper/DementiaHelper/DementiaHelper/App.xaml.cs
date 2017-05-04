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
        //Setup names for you navigation stacks
        public class NavigationStacks
        {
            public static string LoginNavigationStack = "LoginNavigationStack";
            public static string MainAppStack = "MainAppStack";
        }
        public static string AppName { get { return "Dementia Helper"; } }
        // public static string UserName { get { return "John"; } }
        public ApplicationUser ApplicationUser { get; set; }
        //  public static string Password { get { return "password"; } }
        static FreshMvvm.FreshMasterDetailNavigationContainer MasterDetailNav { get; set; }
        static FreshMvvm.FreshNavigationContainer LoginNavigationContainer { get; set; }
        public App()
        {
            
            InitializeComponent();
        }
        static public async Task<bool> LoginAsync(string Email, string Password)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var encoded = JWTService.Encode(new Dictionary<string, object>
                      {
                        {"email",Email},
                       {"password", Password}
                     });
                    client.DefaultRequestHeaders.Add("token", encoded);
                    var result = await client.GetAsync(new Uri("http://dementiahelper.azurewebsites.net/api/account/login"));
                    var decoded = JWTService.Decode(await result.Content.ReadAsStringAsync());
                    if (App.MapToApplicationUser(decoded))
                    {
                        DependencyService.Get<ICredentialsService>().DeleteCredentials();
                        DependencyService.Get<ICredentialsService>().SaveCredentials(Email, Password);
                        return true;
                    }
                    return false;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        static public bool Login(string Email, string Password)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var encoded = JWTService.Encode(new Dictionary<string, object>
                      {
                        {"email",Email},
                       {"password", Password}
                     });
                    client.DefaultRequestHeaders.Add("token", encoded);
                    var result = client.GetAsync(new Uri("http://dementiahelper.azurewebsites.net/api/account/login")).Result;
                    var decoded = JWTService.Decode(result.Content.ReadAsStringAsync().Result);
                    if (App.MapToApplicationUser(decoded))
                    {
                        DependencyService.Get<ICredentialsService>().DeleteCredentials();
                        DependencyService.Get<ICredentialsService>().SaveCredentials(Email, Password);
                        return true;
                    }
                    return false;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }


        static public bool MapToApplicationUser(IDictionary<string, object> dict)
        {
            if (dict.ContainsKey("Password") || dict.ContainsKey("UserExists") || dict.ContainsKey("ErrorRole")) { return false; }
            var User = dict["User"] as JContainer;

            var ApplicationUser = new ApplicationUser()
            {
                ApplicationUserId = User.SelectToken("ApplicationUserId").ToObject<int>(),
                Email = User.SelectToken("Email")?.ToObject<string>(),
                Salt = User.SelectToken("Salt")?.ToObject<string>(),
                Hash = User.SelectToken("Hash")?.ToObject<string>(),
                FirstName = User.SelectToken("FirstName")?.ToObject<string>(),
                LastName = User.SelectToken("LastName")?.ToObject<string>(),
                RoleId = User.SelectToken("RoleId").ToObject<int>(),
                GroupId = User.SelectToken("ChatGroupId").ToObject<int?>(),
                Description = User.SelectToken("Description")?.ToObject<string>(),
                Phone = User.SelectToken("Phone").ToObject<int?>()
            };
            switch (ApplicationUser.RoleId)
            {
                case 1:
                    ApplicationUser.ConnectionId = dict["ConnectionId"].ToString();
                    ApplicationUser.CitizenId = ApplicationUser.ApplicationUserId;
                    break;
                case 2:
                    ApplicationUser.CitizenId = dict["CitizenId"] != null ? Convert.ToInt32(dict["CitizenId"]) : (int?)null;
                    break;
                case 3:
                    if (!dict.ContainsKey("CitizenIds")) break;
                    var list = dict["CitizenIds"] as IList;
                    ApplicationUser.CitizenList = MapToCitizenList(list);
                    break;
            }

            App.Current.Properties["ApplicationUser"] = ApplicationUser;
            return true;
        }

        private static ObservableCollection<Citizen> MapToCitizenList(IList list)
        {
            var tempCitizenList = new ObservableCollection<Citizen>();
            foreach (var obj in list)
            {
                var jsonContainer = obj as JContainer;
                tempCitizenList.Add(new Citizen() {CitizenId= jsonContainer.SelectToken("CitizenId").ToObject<int>(), FirstName= jsonContainer.SelectToken("ApplicationUser").SelectToken("FirstName").ToObject<string>(),LastName= jsonContainer.SelectToken("ApplicationUser").SelectToken("LastName").ToObject<string>()});

            }
            return tempCitizenList;
        }

        static public void SetLoginPageContainer()
        {
            var Login = FreshMvvm.FreshPageModelResolver.ResolvePageModel<LoginPageModel>();
            LoginNavigationContainer = new FreshMvvm.FreshNavigationContainer(Login, NavigationStacks.LoginNavigationStack);
        }

        static public void SetMasterDetailToRole()
        {

            var User = (ApplicationUser)App.Current.Properties["ApplicationUser"];
            switch (User.RoleId)
            {
                case 1:
                    {
                        MasterDetailNav = new FreshMvvm.FreshMasterDetailNavigationContainer(NavigationStacks.MainAppStack);
                        MasterDetailNav.Init("Menu");
                        MasterDetailNav.AddPage<CitizenHomePageModel>(AppResources.CitizenHomeTitle, null);
                        MasterDetailNav.AddPage<ContactListPageModel>(AppResources.ContactListTitle, null);
                        MasterDetailNav.AddPage<ImageGalleryPageModel>(AppResources.ImageGalleryTitle, null);
                        MasterDetailNav.AddPage<ShoppingListPageModel>(AppResources.ShoppingListTitle, null);
                        MasterDetailNav.AddPage<ChatPageModel>(AppResources.ChatTitle, null);
                        MasterDetailNav.AddPage<CalenderPageModel>(AppResources.CalenderTitle, null);
                        MasterDetailNav.AddPage<AccountInformationPageModel>(AppResources.AccountInformationTitle, null);
                        MasterDetailNav.AddPage<ConnectToNursingHomePageModel>(AppResources.ConnectToNursingHomeTitle, null);
                        MasterDetailNav.AddPage<SettingsPageModel>(AppResources.SettingsTitle, null);

                        break;
                    }
                case 2:
                    {
                        MasterDetailNav = new FreshMvvm.FreshMasterDetailNavigationContainer(NavigationStacks.MainAppStack);
                        MasterDetailNav.Init("Menu");
                        MasterDetailNav.AddPage<ConnectToCitizenPageModel>(AppResources.ConnectToCitizenTitle, null);
                        MasterDetailNav.AddPage<ContactListPageModel>(AppResources.ContactListTitle, null);
                        MasterDetailNav.AddPage<ImageGalleryPageModel>(AppResources.ImageGalleryTitle, null);
                        MasterDetailNav.AddPage<ShoppingListPageModel>(AppResources.ShoppingListTitle, null);
                        MasterDetailNav.AddPage<ChatPageModel>(AppResources.ChatTitle, null);
                        MasterDetailNav.AddPage<CalenderPageModel>(AppResources.CalenderTitle, null);
                        MasterDetailNav.AddPage<AccountInformationPageModel>(AppResources.AccountInformationTitle, null);
                        MasterDetailNav.AddPage<SettingsPageModel>(AppResources.SettingsTitle, null);
                        break;
                    }
                case 3:
                    {
                        MasterDetailNav = new FreshMvvm.FreshMasterDetailNavigationContainer(NavigationStacks.MainAppStack);
                        MasterDetailNav.Init("Menu");
                        MasterDetailNav.AddPage<ChooseCitizenPageModel>(AppResources.ChooseCitizenTitle, null);
                        MasterDetailNav.AddPage<ContactListPageModel>(AppResources.ContactListTitle, null);
                        MasterDetailNav.AddPage<ShoppingListPageModel>(AppResources.ShoppingListTitle, null);
                        MasterDetailNav.AddPage<ChatPageModel>(AppResources.ChatTitle, null);
                        MasterDetailNav.AddPage<CalenderPageModel>(AppResources.CalenderTitle, null);

                        MasterDetailNav.AddPage<AccountInformationPageModel>(AppResources.AccountInformationTitle, null);
                        MasterDetailNav.AddPage<NotePageModel>(AppResources.NoteTitle, null);
                        MasterDetailNav.AddPage<SettingsPageModel>(AppResources.SettingsTitle, null);
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
              if ( DependencyService.Get<ICredentialsService>().Authenticate())
                {
                    App.SetMasterDetailToRole();
                    MainPage = MasterDetailNav;
                // CoreMethods.SwitchOutRootNavigation(App.NavigationStacks.MainAppStack);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DementiaHelper.Extensions;
using DementiaHelper.Model;
using DementiaHelper.Services;
using Xamarin.Forms;

namespace DementiaHelper.PageModels
{
    public class AccountInformationPageModel : FreshMvvm.FreshBasePageModel
    {
        public const string URI_BASE = "http://dementiahelper.azurewebsites.net/api/values/getspecific/";
        public const string URI_BASE_TEST = "http://localhost:29342/api/values/getspecific/";
        public UserInformation User { get; set; }
        public bool IsCitizen { get; set; }
        public string CitizenId { get; set; }
        public bool EditButton { get; set ; }
        public ICommand GoToEditAccountInformationCommand { get; protected set; }
        public ICommand BackCommand { get; protected set; }


        public AccountInformationPageModel()
        {
            Device.BeginInvokeOnMainThread(async() =>
            {
                User  = await GetProfile("test@email.com");
                EditButton = User.Id == "test@email.com";
            });
            CheckIfCitizen((ApplicationUser)App.Current.Properties["ApplicationUser"]);
            GoToEditAccountInformationCommand = new Command(async () => await GoToEditAccountInformation());
            BackCommand = new Command(async () => await Back());
        }
        private void CheckIfCitizen(ApplicationUser user)
        {
            if (user.RoleId == 1)
            {
                IsCitizen = true;
                CitizenId = "abcdefg";
            }
            else
            {
                IsCitizen = false;
            }
        }
        async Task GoToEditAccountInformation()
        {
            await CoreMethods.PushPageModel<EditAccountInformationPageModel>(User);
        }
        async Task Back()
        {
            await CoreMethods.PopPageModel();
        }

        async Task<UserInformation> GetProfile(string email)
        {
            var values = new Dictionary<string, object>
            {
                {"Email", email}
            };
            
            using (HttpClient h = new HttpClient())
            {
                try
                {
                    var encoded = JWTService.Encode(values);
                    var result = await h.GetStringAsync(new Uri(URI_BASE + encoded));
                    var decoded = JWTService.Decode(result);

                    return new UserInformation() { FirstName = decoded["FirstName"]?.ToString(), LastName = decoded["LastName"]?.ToString(), Email = decoded["Email"]?.ToString(), Description = decoded["Description"]?.ToString() };
                }
                catch (Exception)
                {
                    return new UserInformation() { FirstName = "FirstName", LastName = "LastName", Email = "Email", Description = "Description" }; //Test string
                    
                }
            }
        }



    }
}

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
        private const string URI_BASE = "http://dementiahelper.azurewebsites.net/api/account/getuser/";
        private const string URI_BASE_TEST = "http://localhost:29342/api/account/getuser/";
        private UserInformation ShowedUser { get; set; }
        private readonly ApplicationUser User = (ApplicationUser)App.Current.Properties["ApplicationUser"];
        private bool IsCitizen { get; set; }
        private int? CitizenId { get; set; }
        private bool Editbutton { get; set ; }
        private ICommand GoToEditAccountInformationCommand { get; set; }
        private ICommand BackCommand { get; set; }


        public AccountInformationPageModel()
        {
            Editbutton = false;
            ShowedUser = new UserInformation();
            CheckIfCitizen((ApplicationUser)App.Current.Properties["ApplicationUser"]);
            GoToEditAccountInformationCommand = new Command(async () => await GoToEditAccountInformation());
            BackCommand = new Command(async () => await Back());
        }
        private void CheckIfCitizen(ApplicationUser user)
        {
            if (user.RoleId == 1)
            {
                IsCitizen = true;
                CitizenId = user.CitizenId;
            }
            else
            {
                IsCitizen = false;
            }
        }

        private async Task GoToEditAccountInformation()
        {
            await CoreMethods.PushPageModel<EditAccountInformationPageModel>(User);
        }
        private async Task Back()
        {
            await CoreMethods.PopPageModel();
        }

        private async Task<UserInformation> GetProfile(string email)
        {
            var payload = new Dictionary<string, object>
            {
                {"email", email}
            };
            
            using (HttpClient h = new HttpClient())
            {
                try
                {
                    var encoded = JWTService.Encode(payload);
                    var result = await h.GetStringAsync(new Uri(URI_BASE + encoded));
                    var decoded = JWTService.Decode(result);

                    return new UserInformation() { FirstName = decoded["firstName"]?.ToString(), LastName = decoded["lastName"]?.ToString(), Email = decoded["email"]?.ToString(), Description = decoded["description"]?.ToString() };
                    //return new UserInformation() { FirstName = "FirstName", LastName = "LastName", Email = "Email", Description = "Description" }; //Test string
                }
                catch (Exception)
                {
                    return new UserInformation() { FirstName = "FirstName", LastName = "LastName", Email = "Email", Description = "Description" }; //Test string
                    
                }
            }
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            Device.BeginInvokeOnMainThread(async () =>
            {
                ShowedUser = await GetProfile(User.Email);
                Editbutton = ShowedUser.Email == User.Email;
            });
        }
    }
}

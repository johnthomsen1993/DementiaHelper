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
using PropertyChanged;
using Xamarin.Forms;

namespace DementiaHelper.PageModels
{
    [ImplementPropertyChanged]
    public class AccountInformationPageModel : FreshMvvm.FreshBasePageModel
    {
        private const string URI_BASE = "http://dementiahelper.azurewebsites.net/api/account/getuser/";
        private const string URI_BASE_TEST = "http://localhost:29342/api/account/getuser/";
        public UserInformation ShowedUser { get; set; }
        public bool IsCitizen { get; set; }
        public string CitizenId { get; set; }
        public bool Editbutton { get; set ; }
        public ICommand GoToEditCommand { get; set; }


        public AccountInformationPageModel()
        {
            GoToEditCommand = new Command(async () => await GoToEdit());
        }

        private void CheckIfCitizen(ApplicationUser user)
        {
            if (user.RoleId == 1)
            {
                IsCitizen = true;
                CitizenId = user.ConnectionId; 
            }
            else
            {
                IsCitizen = false;
            }
        }

        private async Task GoToEdit()
        {
            await CoreMethods.PushPageModel<EditAccountInformationPageModel>(ShowedUser);
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

                    return new UserInformation()
                    {
                        FirstName = decoded["firstName"]?.ToString(),
                        LastName = decoded["lastName"]?.ToString(),
                        Email = decoded["email"]?.ToString(),
                        Description = decoded["description"]?.ToString(),
                        Phone = decoded["phone"]?.ToString()
                    };
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
            CheckIfCitizen((ApplicationUser)App.Current.Properties["ApplicationUser"]);
            Device.BeginInvokeOnMainThread(async () =>
            {
                ShowedUser = await GetProfile(((ApplicationUser)App.Current.Properties["ApplicationUser"]).Email);
                Editbutton = ShowedUser.Email == ((ApplicationUser)App.Current.Properties["ApplicationUser"]).Email;
            });
        }
    }
}

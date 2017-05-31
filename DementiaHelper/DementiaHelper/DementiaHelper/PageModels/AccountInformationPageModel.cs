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
        #region ViewModel Properties
        public UserInformation ShowedUser { get; set; }
        public bool IsCitizen { get; set; }
        public string CitizenId { get; set; }
        public bool Editbutton { get; set ; }
        public ICommand GoToEditCommand { get; set; }
        #endregion


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
        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            CheckIfCitizen((ApplicationUser)App.Current.Properties["ApplicationUser"]);
            Device.BeginInvokeOnMainThread(async () =>
            {
                ShowedUser = await ModelAccessor.Instance.AccountController.GetProfile(((ApplicationUser)App.Current.Properties["ApplicationUser"]).Email);
                Editbutton = ShowedUser.Email == ((ApplicationUser)App.Current.Properties["ApplicationUser"]).Email;
            });
        }
    }
}

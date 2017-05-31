using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using DementiaHelper.Extensions;
using DementiaHelper.Model;
using DementiaHelper.Resx;
using DementiaHelper.Services;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Agreement.JPake;
using PropertyChanged;
using Xamarin.Forms;

namespace DementiaHelper.PageModels
{
    [ImplementPropertyChanged]
    public class EditAccountInformationPageModel : FreshMvvm.FreshBasePageModel
    {
        public UserInformation UpdatedUser { get; set; }
        public ICommand SaveUpdateUserInformationCommand { get; protected set; }
        public ICommand CancelUpdateOfUserInformationCommand { get; protected set; }

        public EditAccountInformationPageModel()
        {
            this.SaveUpdateUserInformationCommand = new Command(async () => await SaveUpdateUserInformation());
            this.CancelUpdateOfUserInformationCommand = new Command(async () => await Cancel());
        }

        public override void Init(object initData)
        {
            base.Init(initData);
            UpdatedUser = (UserInformation)initData;
        }
        async Task SaveUpdateUserInformation()
        {
            if (!IsValidEmail(UpdatedUser.Email))
            {
                await CoreMethods.DisplayAlert(AppResources.General_InvalidEmailTitle, AppResources.General_InvalidEmailText, AppResources.General_Ok);
            }
            else if (UpdatedUser.FirstName == "" || UpdatedUser.LastName == "")
            {
                await CoreMethods.DisplayAlert(AppResources.General_NullTitle, AppResources.General_MissingName, AppResources.General_Ok);
            }
            else
            {
                await ModelAccessor.Instance.AccountController.SaveUpdateUserInformation(UpdatedUser,
                        ((ApplicationUser)App.Current.Properties["ApplicationUser"]).Email);
                await CoreMethods.PopPageModel();
                
            }
        }

        async Task Cancel()
        {
           await  CoreMethods.PopPageModel();
        }

        private bool IsValidEmail(string inputEmail)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            return re.IsMatch(inputEmail);
        }
    }
}

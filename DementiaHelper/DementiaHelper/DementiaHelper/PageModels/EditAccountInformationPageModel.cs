using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DementiaHelper.Extensions;
using DementiaHelper.Model;
using DementiaHelper.Services;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Agreement.JPake;
using PropertyChanged;
using Xamarin.Forms;

namespace DementiaHelper.PageModels
{
    [ImplementPropertyChanged]
    class EditAccountInformationPageModel : FreshMvvm.FreshBasePageModel
    {
        public const string URI_BASE = "http://dementiahelper.azurewebsites.net/api/account/update/";
        public const string URI_BASE_TEST = "http://localhost:29342/api/account/update/";
        public UserInformation UpdatedUser { get; set; }
        private ApplicationUser User = (ApplicationUser)App.Current.Properties["ApplicationUser"];
        public ICommand SaveCommand { get; protected set; }
        public ICommand CancelCommand { get; protected set; }

        public EditAccountInformationPageModel()
        {
            this.SaveCommand = new Command(async () => await Save());
            this.CancelCommand = new Command(async () => await Cancel());
        }

        public override void Init(object initData)
        {
            base.Init(initData);
            UpdatedUser = (UserInformation)initData;
        }
        async Task Save()
        {
            var payload = new Dictionary<string, object>
            {
                {"firstName", UpdatedUser.FirstName},
                {"lastName", UpdatedUser.LastName},
                {"email", UpdatedUser.Email},
                {"description", UpdatedUser.Description},
                {"phone", UpdatedUser.Phone},
                {"oldEmail", User.Email }
            };

            var encoded = JWTService.Encode(payload);

            using (HttpClient h = new HttpClient())
            {
                var values = new Dictionary<string, string> {{"token", encoded}};
                var content = new FormUrlEncodedContent(values);
                var result = h.PutAsync(new Uri(URI_BASE), content).Result;
                var decoded = JWTService.Decode(await result.Content.ReadAsStringAsync());
                if (decoded != null)
                {
                    if (Convert.ToBoolean(decoded["UserUpdated"]))
                    {
                        User.FirstName = UpdatedUser.FirstName;
                        User.LastName = UpdatedUser.LastName;
                        User.Description = UpdatedUser.Description;
                        User.Email = UpdatedUser.Email;
                        User.Phone = UpdatedUser.Phone;
                    }
                }
            }

            await CoreMethods.PopPageModel();
        }
        async Task Cancel()
        {
            await CoreMethods.PopPageModel();
        }

        
    }
}

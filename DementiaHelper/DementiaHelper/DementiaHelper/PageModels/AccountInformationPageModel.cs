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
        public bool EditButton { get; set; }
        public ICommand GoToEditAccountInformationCommand { get; protected set; }
        public ICommand BackCommand { get; protected set; }


        public AccountInformationPageModel()
        {
            
            var values = new Dictionary<string, object>
            {
                {"Email", "test@gmail.com"}
            };

            using (HttpClient h = new HttpClient())
            {
                var encoded = JWTService.Encode(values);
                StringContent content = new StringContent(encoded);
                var result = h.PostAsync(new Uri(URI_BASE), content).Result;
                var decoded = JWTService.Decode(result.Content.ReadAsStringAsync().ToString());

                User = new UserInformation() { FirstName = decoded["FirstName"]?.ToString(), LastName = decoded["LastName"]?.ToString(), Email = decoded["Email"]?.ToString(), Description = decoded["Description"]?.ToString() };
            }
            
            EditButton = User.Id == "test@email.com";
            GoToEditAccountInformationCommand = new Command(async () => await GoToEditAccountInformation());
            BackCommand = new Command(async () => await Back());
            
        }

        async Task GoToEditAccountInformation()
        {
            await CoreMethods.PushPageModel<EditAccountInformationPageModel>(User);
        }
        async Task Back()
        {
            await CoreMethods.PopPageModel();
        }

    }
}

using DementiaHelper.Model;
using DementiaHelper.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DementiaHelper.Resx;
using Xamarin.Forms;

namespace DementiaHelper.PageModels
{
   public class ConnectToNursingHomePageModel : FreshMvvm.FreshBasePageModel
    {

        public const string URI_BASE = "http://dementiahelper.azurewebsites.net/api/account/connectcitizentocenter/";
        public string ConnectionId { get; set; }
        public ICommand ConnectToNursingHomeCommand { get; protected set; }
        public ConnectToNursingHomePageModel()
        {
            ConnectToNursingHomeCommand = new Command(async () => await ConnectToNursingHome());

        }

        private async Task ConnectToNursingHome()
        {
            using (var client = new HttpClient())
            {
                var encoded = JWTService.Encode(new Dictionary<string, object>() { { "CitizenId", ((ApplicationUser)App.Current.Properties["ApplicationUser"]).ApplicationUserId }, { "ConnectionId", ConnectionId } });
                var values = new Dictionary<string, string> { { "token", encoded } };
                var content = new FormUrlEncodedContent(values);
                var result = await client.PutAsync(new Uri(URI_BASE), content);
                var decoded = JWTService.Decode(await result.Content.ReadAsStringAsync());
                if ((bool)decoded["Connected"])
                {
                    ConnectionId = "";
                    await CoreMethods.SwitchSelectedMaster<CitizenHomePageModel>();
                }
                else
                {
                    await CoreMethods.DisplayAlert(AppResources.Connection_ErrorTitle, AppResources.Connection_ErrorText, AppResources.General_Ok);
                }
            }
        }
    }
}

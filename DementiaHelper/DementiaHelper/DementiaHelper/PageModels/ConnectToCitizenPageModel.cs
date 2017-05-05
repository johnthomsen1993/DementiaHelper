using DementiaHelper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DementiaHelper.Model;
using DementiaHelper.Services;
using DementiaHelper.PageModels;
using DementiaHelper.Resx;
using Xamarin.Forms;

namespace DementiaHelper.PageModels
{
    public class ConnectToCitizenPageModel : FreshMvvm.FreshBasePageModel
    {
        public const string URI_BASE = "http://dementiahelper.azurewebsites.net/api/account/connecttocitizen/";
        public string ConnectionId { get; set; }
        public ICommand ConnectToCitizenCommand { get; protected set; }
        public ConnectToCitizenPageModel() {
            ConnectToCitizenCommand = new Command(async () => await ConnectToCitizen());

        }

        private async Task ConnectToCitizen()
        {
            using (var client = new HttpClient())
            {
                var encoded = JWTService.Encode(new Dictionary<string, object>() { { "RelativeId", ((ApplicationUser)App.Current.Properties["ApplicationUser"]).ApplicationUserId }, { "ConnectionId", ConnectionId } });
                var values = new Dictionary<string, string> { { "token", encoded } };
                var content = new FormUrlEncodedContent(values);
                var result = await client.PutAsync(new Uri(URI_BASE), content);
                var decoded = JWTService.Decode(await result.Content.ReadAsStringAsync());
                if (decoded != null)
                {
                    if (!decoded.ContainsKey("Connected"))
                    {
                        App.MapToApplicationUser(decoded);
                        ConnectionId = "";
                        await CoreMethods.SwitchSelectedMaster<CalenderPageModel>();
                    }
                    else
                    {
                        await CoreMethods.DisplayAlert(AppResources.Account_ConnectErrorTitle, AppResources.Account_ConnectErrorText, AppResources.General_Ok); // TODO: find ud af om rigtig externalization
                        await CoreMethods.DisplayAlert("Fejl", "Det var ikke muligt at forbinde til borgeren, check venligst at dit indtasted borger id passer overens med borgerens id i borgerens konto view", "Ok");

                    }
                }
                else
                {
                    await CoreMethods.DisplayAlert(AppResources.Account_ConnectErrorTitle, AppResources.Account_ConnectErrorText, AppResources.General_Ok);
                }
            }
        }
    }
}

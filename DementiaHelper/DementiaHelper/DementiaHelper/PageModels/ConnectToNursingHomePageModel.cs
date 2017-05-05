﻿using DementiaHelper.Model;
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
                var user = (ApplicationUser)App.Current.Properties["ApplicationUser"];
                var encoded = JWTService.Encode(new Dictionary<string, object>() { { "CitizenId", user.ApplicationUserId }, { "ConnectionId", ConnectionId } });
                var values = new Dictionary<string, string> { { "content", encoded } };
                var content = new FormUrlEncodedContent(values);
                var result = await client.PutAsync(new Uri(URI_BASE), content);
                var decoded = JWTService.Decode(await result.Content.ReadAsStringAsync());
                if ((bool)decoded["Connected"])
                {
                    ConnectionId = "";
                    await CoreMethods.SwitchSelectedMaster<CalenderPageModel>();
                }
                else
                {
                    await CoreMethods.DisplayAlert(AppResources.Account_ConnectErrorTitle, AppResources.Account_ConnectErrorText, AppResources.General_Ok);
                }
            }
        }
    }
}

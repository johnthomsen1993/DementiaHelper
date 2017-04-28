﻿using DementiaHelper.Extensions;
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
                var user = (ApplicationUser)App.Current.Properties["ApplicationUser"];
                var encoded = JWTService.Encode(new Dictionary<string, object>() { { "RelativeId", user.ApplicationUserId }, { "ConnectionId", ConnectionId } });
                var values = new Dictionary<string, string> { { "content", encoded } };
                var content = new FormUrlEncodedContent(values);
                var result = await client.PutAsync(new Uri(URI_BASE), content);
                var decoded = JWTService.Decode(await result.Content.ReadAsStringAsync());
                //TODO: do something with decoded result
            }
        }
    }
}

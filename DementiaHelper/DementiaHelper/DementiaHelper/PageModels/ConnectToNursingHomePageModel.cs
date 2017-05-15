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

            var decoded = await ModelAccessor.Instance.AccountController.ConnectToNursingHome(ConnectionId);

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

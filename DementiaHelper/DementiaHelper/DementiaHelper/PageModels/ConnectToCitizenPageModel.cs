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
        #region ViewModel Properties
        public string ConnectionId { get; set; }
        public ICommand ConnectToCitizenCommand { get; protected set; }
        #endregion
        public ConnectToCitizenPageModel() {
            ConnectToCitizenCommand = new Command(async () => await ConnectToCitizen());

        }

        private async Task ConnectToCitizen()
        {
                var decoded = await ModelAccessor.Instance.AccountController.ConnectToCitizen(ConnectionId);
                if (decoded != null)
                {
                    if (!decoded.ContainsKey("Connected"))
                    {
                        ModelAccessor.Instance.AccountController.MapToApplicationUser(decoded);
                        ConnectionId = "";
                        await CoreMethods.SwitchSelectedMaster<CalendarPageModel>();
                    }
                    else
                    {
                        await CoreMethods.DisplayAlert(AppResources.Account_ConnectIdErrorTitle, AppResources.Account_ConnectIdErrorText, AppResources.General_Ok);

                    }
                }
                else
                {
                    await CoreMethods.DisplayAlert(AppResources.Connection_ErrorTitle, AppResources.Connection_ErrorText, AppResources.General_Ok);
                }
            }
        
    }
}

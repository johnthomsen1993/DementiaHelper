using DementiaHelper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DementiaHelper.Services;
using DementiaHelper.PageModels;
using Xamarin.Forms;

namespace DementiaHelper.PageModels
{
    public class ConnectToCitizenPageModel : FreshMvvm.FreshBasePageModel
    {
        public string CitizenId { get; set; }
        public ICommand ConnectToCitizenCommand { get; protected set; }
        public ConnectToCitizenPageModel() {
            ConnectToCitizenCommand = new Command(async () => await ConnectToCitizen());

        }

        async Task ConnectToCitizen() {
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DementiaHelper.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CitizenHomePage : ContentPage
    {
        public CitizenHomePage()
        {
            InitializeComponent();
            //Device.StartTimer(TimeSpan.FromSeconds(1), () => {
            //    Device.BeginInvokeOnMainThread(() => yourLabel.Text = DateTime.Now.ToString());
            //    return true;
            //});
        }
    }
}

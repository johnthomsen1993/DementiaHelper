using DementiaHelper.Resx;
using DementiaHelper.Services;
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
        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () => {
                var result = await this.DisplayAlert(AppResources.Warning + "!", AppResources.AreYouSureThatYouWantToCloseThisApplication, AppResources.YesText, AppResources.NoText);
                if (result)
                {
                    INativeService nativeHelper = DependencyService.Get<INativeService>();
                    if (nativeHelper != null)
                    {
                        nativeHelper.CloseApp();
                    }
                }
            });

            return true;
        }
    }
}

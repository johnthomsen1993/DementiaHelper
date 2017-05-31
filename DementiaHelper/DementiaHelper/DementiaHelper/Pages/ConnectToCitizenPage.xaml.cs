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
    public partial class ConnectToCitizenPage : ContentPage
    {
        private double width;
        private double height;
        public ConnectToCitizenPage()
        {
            InitializeComponent();
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
        /// <summary>
        /// how to handle rotation changes in xamarin.forms have been adapted from
        /// https://developer.xamarin.com/guides/xamarin-forms/user-interface/layouts/device-orientation/
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (width != this.width || height != this.height)
            {
                this.width = width;
                this.height = height;
                if (width > height)
                {
                    outerStack.Orientation = StackOrientation.Horizontal;
                    secondInnerStack.VerticalOptions = LayoutOptions.StartAndExpand;
                    imageElderlyHands.HeightRequest = 200;
                }
                else
                {
                    outerStack.Orientation = StackOrientation.Vertical;
                    secondInnerStack.VerticalOptions = LayoutOptions.CenterAndExpand;
                    imageElderlyHands.HeightRequest = 200;
                }
            }
        }
    }
}

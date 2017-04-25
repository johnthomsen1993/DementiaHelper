using DementiaHelper.Resx;
using DementiaHelper.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DementiaHelper.Pages
{
    public partial class CreateAccountPage : ContentPage
    {


        private double width;
        private double height;
        public CreateAccountPage()
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
                    imageElderlyHands.HeightRequest = 120;
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
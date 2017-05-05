using DementiaHelper.Resx;
using DementiaHelper.Services;
using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace DementiaHelper.Pages
{
    public partial class LoginPage : ContentPage
    {
        private double width;
        private double height;
        public LoginPage()
        {
            InitializeComponent();
        }


    protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () => {
                var result = await this.DisplayAlert(AppResources.Warning+"!", AppResources.AreYouSureThatYouWantToCloseThisApplication, AppResources.YesText, AppResources.NoText);
                if (result) {
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
                    if (Device.Idiom == TargetIdiom.Phone)
                    {
                        outerStack.Orientation = StackOrientation.Horizontal;
                        imageElderlyHands.HeightRequest = 120;
                    }
                    else if (Device.Idiom == TargetIdiom.Tablet)
                    {
                        //outerStackTablet
                        imageElderlyHandsTablet.HeightRequest = 400;
                    }
                }
                else
                {
                    if (Device.Idiom == TargetIdiom.Phone)
                    {
                        outerStack.Orientation = StackOrientation.Vertical;
                        imageElderlyHands.HeightRequest = 200;
                    }
                    else if(Device.Idiom == TargetIdiom.Tablet)
                    {
                        imageElderlyHandsTablet.HeightRequest = 400;
                    }
                }
            }
        }
    }
}

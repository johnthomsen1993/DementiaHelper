using DementiaHelper.PageModels;
using DementiaHelper.Resx;
using DementiaHelper.Services;
using Syncfusion.SfSchedule.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace DementiaHelper.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarPage : ContentPage
    {
        private double width;
        private double height;
        public CalendarPage()
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
                    if (Device.Idiom == TargetIdiom.Phone)
                    {
                        AbsoluteLayout.SetLayoutBounds(TodaysAppointments, new Rectangle(.50, .10, 500, 79));
                        AbsoluteLayout.SetLayoutBounds(WeeksAppointments, new Rectangle(.50, .50, 500, 79));
                        AbsoluteLayout.SetLayoutBounds(MonthsAppointments, new Rectangle(.50, .90, 500, 79));
                    }
                    else
                    {
                        AbsoluteLayout.SetLayoutBounds(TodaysAppointments, new Rectangle(.50, .25, 500, 100));
                        AbsoluteLayout.SetLayoutBounds(WeeksAppointments, new Rectangle(.50, .50, 500, 100));
                        AbsoluteLayout.SetLayoutBounds(MonthsAppointments, new Rectangle(.50, .75, 500, 100));
                    }
                }
                else
                {
                    if (Device.Idiom == TargetIdiom.Phone)
                    {
                        AbsoluteLayout.SetLayoutBounds(TodaysAppointments, new Rectangle(.50, .25, 300, 100));
                        AbsoluteLayout.SetLayoutBounds(WeeksAppointments, new Rectangle(.50, .50, 300, 100));
                        AbsoluteLayout.SetLayoutBounds(MonthsAppointments, new Rectangle(.50, .75, 300, 100));
                    }
                    else
                    {
                        AbsoluteLayout.SetLayoutBounds(TodaysAppointments, new Rectangle(.50, .25, 500, 150));
                        AbsoluteLayout.SetLayoutBounds(WeeksAppointments, new Rectangle(.50, .50, 500, 150));
                        AbsoluteLayout.SetLayoutBounds(MonthsAppointments, new Rectangle(.50, .75, 500, 150));
                    }

                }
            }
        }

    }

}

using DementiaHelper.Model;
using DementiaHelper.Services;
using Newtonsoft.Json.Linq;
using PropertyChanged;
using Syncfusion.SfSchedule.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DementiaHelper.PageModels
{
    [ImplementPropertyChanged]
    public class CitizenHomePageModel :FreshMvvm.FreshBasePageModel
    {
        #region ViewModel Properties
        public DateTime CurrentTime { get; set; }
        public string Weekday { get; set; }
        public string Month { get; set; }
        public ICommand GoToCalendarDayViewCommand { get; protected set; }
        public ImageSource PictureOfWhoIsVisiting { get; set; }
        public ScheduleAppointment Appointment { get; set; }
        #endregion

        public CitizenHomePageModel()
        {
            var test = DateTime.Now;
            GoToCalendarDayViewCommand = new Command(async () => await GoToCalendarDayView());
            Weekday = FirstLetterToUpper(test.ToString("dddd", new CultureInfo("da-DK")));
            Month = FirstLetterToUpper( test.ToString("MMMM", new CultureInfo("da-DK")));
            PictureOfWhoIsVisiting = ImageSource.FromFile("FakeProfilBillede.png");
            Device.StartTimer(TimeSpan.FromSeconds(1), () => {
                    CurrentTime = DateTime.Now;
                    return true;
                });
        }


        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            Device.BeginInvokeOnMainThread(async () => {
                Appointment = await ModelAccessor.Instance.CalendarController.GetAppointment(); 
            });
        }

        private async Task GoToCalendarDayView()
        {
          await  CoreMethods.PushPageModel<DayCalendarPageModel>();
        }

        private string FirstLetterToUpper(string str)  // made this one since CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower()); does not exist in xamarin.forms pcl yet, which would have been the proper way to do it
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }
    }
}

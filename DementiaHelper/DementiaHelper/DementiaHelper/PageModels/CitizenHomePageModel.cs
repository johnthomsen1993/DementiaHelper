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
     
        public DateTime CurrentTime { get; set; }
       
        public string Weekday { get; set; }
        public string Month { get; set; }
        public ICommand GoToCalendarDayViewCommand { get; protected set; }
        public ImageSource PictureOfWhoIsVisiting { get; set; }
        public ScheduleAppointment Appointment { get; set; }

        public CitizenHomePageModel()
        {
            var test = DateTime.Now;
            GoToCalendarDayViewCommand = new Command(async (id) => await GoToCreateShoppingItem());
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
                Appointment = await GetNextAppointment();
            });
        }
        private async Task<ScheduleAppointment> GetAppointments()
        {
            if (((ApplicationUser)App.Current.Properties["ApplicationUser"]).CitizenId == null) { return new ScheduleAppointment(); }
            using (var client = new HttpClient())
            {
                try
                {
                    var encoded = JWTService.Encode(new Dictionary<string, object>() { { "CitizenId", ((ApplicationUser)App.Current.Properties["ApplicationUser"]).CitizenId } });
                    var result = await client.GetStringAsync(new Uri("http://dementiahelper.azurewebsites.net/api/values/calendar/latest/" + encoded));
                    var decoded = JWTService.Decode(result);
                    return MapToCalenderAppointments(decoded);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        private ScheduleAppointment MapToCalenderAppointments(IDictionary<string, object> decoded)
        {
           var tempCalenderAppointmentsList = new ScheduleAppointment();
            var jsonContainer = decoded["Appointment"] as JContainer;
            tempCalenderAppointmentsList.Subject = jsonContainer.SelectToken("Subject").ToString();
            tempCalenderAppointmentsList.Color = Color.FromHex(jsonContainer.SelectToken("Color").ToString());
            tempCalenderAppointmentsList.StartTime = jsonContainer.SelectToken("StartTime").ToObject<DateTime>().ToLocalTime();
            tempCalenderAppointmentsList.EndTime = jsonContainer.SelectToken("EndTime").ToObject<DateTime>().ToLocalTime();
            return tempCalenderAppointmentsList;
        }
        private async Task GoToCreateShoppingItem()
        {
          await  CoreMethods.SwitchSelectedMaster<CalenderPageModel>();
        }

        private string FirstLetterToUpper(string str)  // made this one since CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower()); does not exist in xamarin.forms pcl yet, which would have been the proper way to do it
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }

        private async Task<ScheduleAppointment> GetNextAppointment() {
            var ScheduleAppointments = await GetAppointments();
            var currentTime = DateTime.Now;
            return ScheduleAppointments;
        }
    }
}

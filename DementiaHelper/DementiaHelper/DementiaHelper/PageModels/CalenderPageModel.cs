using DementiaHelper.Model;
using DementiaHelper.Pages;
using DementiaHelper.Resx;
using DementiaHelper.Services;
using Newtonsoft.Json.Linq;
using PropertyChanged;
using Syncfusion.SfSchedule.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DementiaHelper.PageModels
{
    [ImplementPropertyChanged]
    public class CalendarPageModel : FreshMvvm.FreshBasePageModel
    {
        public const string URI_BASE = "http://dementiahelper.azurewebsites.net/api/values/calendar/";
        public ICommand GoToDayViewCommand { get; protected set; }
        public ICommand GoToWeekViewCommand { get; protected set; }
        public ICommand GoToMonthViewCommand { get; protected set; }

        public CalendarPageModel()
        {
            this.GoToDayViewCommand = new Command(async () => await GoToDayView());
            this.GoToWeekViewCommand = new Command(async () => await GoToWeekView());
            this.GoToMonthViewCommand = new Command(async () => await GoToMonthView());

        }

        private async Task GoToMonthView()
        {
            await CoreMethods.PushPageModel<MonthCalendarPageModel>();
        }

        private async Task GoToWeekView()
        {
            await CoreMethods.PushPageModel<WeekCalendarPageModel>();
        }

        private async Task GoToDayView()
        {
            await CoreMethods.PushPageModel<DayCalendarPageModel>();
        }

        public static async Task<ObservableCollection<ScheduleAppointment>> GetAppointments(int? id)
        {
            if (id == null) { return new ObservableCollection<ScheduleAppointment>(); }
            using (var client = new HttpClient())
            {
                try
                {
                    var encoded = JWTService.Encode(new Dictionary<string, object>() { { "CitizenId", id } });
                    var result = await client.GetStringAsync(new Uri(URI_BASE + encoded));
                    var decoded = JWTService.Decode(result);
                    return MapToCalenderAppointments(decoded);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }



        /// <summary>
        /// might need to get moved elsewhere
        /// </summary>
        /// <param name="decoded"></param>
        /// <returns></returns>
        public static ObservableCollection<ScheduleAppointment> MapToCalenderAppointments(IDictionary<string, object> decoded)
        {
            ObservableCollection<ScheduleAppointment> tempCalenderAppointmentsList = new ObservableCollection<ScheduleAppointment>();

            var list = decoded.SingleOrDefault(x => x.Key.Equals("Appointments")).Value as IEnumerable<object>;
            foreach (var obj in list)
            {
                var jsonContainer = obj as JContainer;
                tempCalenderAppointmentsList.Add(new ScheduleAppointment()
                {
                    Subject = jsonContainer.SelectToken("Subject").ToString(),
                    Color = Color.FromHex(jsonContainer.SelectToken("Color").ToString()),
                    StartTime = jsonContainer.SelectToken("StartTime").ToObject<DateTime>().ToLocalTime(),
                    EndTime = jsonContainer.SelectToken("EndTime").ToObject<DateTime>().ToLocalTime()
                });
            }
            return tempCalenderAppointmentsList;
        }
    }
}

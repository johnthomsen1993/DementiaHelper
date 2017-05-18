using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DementiaHelper.Resx;
using DementiaHelper.Services;
using Newtonsoft.Json.Linq;
using Syncfusion.SfSchedule.XForms;
using Xamarin.Forms;

namespace DementiaHelper.Model.Controllers
{
    public class CalendarController : ICalendarController
    {

        public async Task<ObservableCollection<ScheduleAppointment>> GetAppointments(int? id)
        {
            if (id == null)
            {
                return new ObservableCollection<ScheduleAppointment>();
            }
            using (var client = new HttpClient())
            {
                try
                {
                    var encoded = JWTService.Encode(new Dictionary<string, object>() {{"CitizenId", id}});
                    var result =
                        await client.GetStringAsync(new Uri(
                            "http://dementiahelper.azurewebsites.net/api/values/calendar/" + encoded));
                    var decoded = JWTService.Decode(result);
                    return MapToCalenderAppointments(decoded);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public ObservableCollection<ScheduleAppointment> MapToCalenderAppointments(IDictionary<string, object> decoded)
        {
            ObservableCollection<ScheduleAppointment> tempCalenderAppointmentsList =
                new ObservableCollection<ScheduleAppointment>();

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

        public async Task<ScheduleAppointment> GetAppointment()
        {
            if (((ApplicationUser) App.Current.Properties["ApplicationUser"]).CitizenId == null)
            {
                return new ScheduleAppointment();
            }
            using (var client = new HttpClient())
            {
                try
                {
                    var encoded = JWTService.Encode(new Dictionary<string, object>()
                    {
                        {"CitizenId", ((ApplicationUser) App.Current.Properties["ApplicationUser"]).CitizenId}
                    });
                    var result =
                        await client.GetStringAsync(new Uri(
                            "http://dementiahelper.azurewebsites.net/api/values/calendar/latest/" + encoded));
                    var decoded = JWTService.Decode(result);
                    return MapToCalenderAppointment(decoded);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public ScheduleAppointment MapToCalenderAppointment(IDictionary<string, object> decoded)
        {
            var tempCalenderAppointment = new ScheduleAppointment();
            var jsonContainer = decoded["Appointment"] as JContainer;
            tempCalenderAppointment.Subject = jsonContainer.SelectToken("Subject").ToString();
            tempCalenderAppointment.Color = Color.FromHex(jsonContainer.SelectToken("Color").ToString());
            tempCalenderAppointment.StartTime = jsonContainer.SelectToken("StartTime").ToObject<DateTime>()
                .ToLocalTime();
            tempCalenderAppointment.EndTime = jsonContainer.SelectToken("EndTime").ToObject<DateTime>().ToLocalTime();
            return tempCalenderAppointment;
        }

        public string GetHexColor(string selecteColorName)
        {
            return selecteColorName == AppResources.Red
                ? "#ff0000"
                : (selecteColorName == AppResources.Blue
                    ? "#0000ff"
                    : (selecteColorName == AppResources.Green ? "#00ff00" : "#FFA2C139"));
        }

        public async Task<IDictionary<string, object>> CreateNewAppointment(string selecteColorName, string description,
            int citizenId, DateTime endTime, DateTime startTime)
        {
            using (var client = new HttpClient())
            {
                var colorText = GetHexColor(selecteColorName);
                var encoded = JWTService.Encode(new Dictionary<string, object>
                {
                    {"CitizenId", citizenId},
                    {"Subject", description},
                    {"Color", colorText},
                    {"StartTime", startTime},
                    {"EndTime", endTime}
                });
                var values = new Dictionary<string, string> {{"token", encoded}};
                var content = new FormUrlEncodedContent(values);
                var result =
                    await client.PutAsync(new Uri("http://dementiahelper.azurewebsites.net/api/values/calendar"),
                        content);
                return JWTService.Decode(await result.Content.ReadAsStringAsync());
            }

        }
    }
}
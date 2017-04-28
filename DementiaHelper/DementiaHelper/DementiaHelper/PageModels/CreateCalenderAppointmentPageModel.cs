using DementiaHelper.Pages;
using DementiaHelper.Services;
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
    public class CreateCalenderAppointmentPageModel : FreshMvvm.FreshBasePageModel
    {
        public ICommand CreateAppointmentCommand { get; protected set; }
        public ICommand CancelCreateAppointmentCommand { get; protected set; }
        public string Description { get; set; }

        private DateTime _date { get; set; }
        public DateTime Date
        {
            get { return _date; }
            set
            {

                _date = value;
                RaisePropertyChanged("Date");
            }
        }

        public ScheduleAppointment Appointment { get; set; }
        private TimeSpan _appointmentStartTimeSpan { get; set; }

        public TimeSpan AppointmentStartTimeSpan
        {
            get { return _appointmentStartTimeSpan; } 
            set {

                _appointmentStartTimeSpan = value;
                RaisePropertyChanged("AppointmentStartTimeSpan");
            }
        }


        private TimeSpan _appointmentEndTimeSpan { get; set; }

        public TimeSpan AppointmentEndTimeSpan
        {
            get { return _appointmentEndTimeSpan; }
            set
            {

                _appointmentEndTimeSpan = value;
                RaisePropertyChanged("AppointmentEndTimeSpan");
            }
        }
        public ObservableCollection<String> ColorList{ get; set; } 






        public CreateCalenderAppointmentPageModel()
        {
            this.CreateAppointmentCommand = new Command( async() => await CreateNewAppointment());
            this.CancelCreateAppointmentCommand = new Command(async () => await CoreMethods.PopPageModel());
            AppointmentStartTimeSpan = new TimeSpan(12, 0, 0);
            AppointmentEndTimeSpan = new TimeSpan(13, 0, 0);
            Description = "";
            Date = DateTime.Now;
            DateTime currentDate = DateTime.Now;
            ColorList = new ObservableCollection<string>() {"Red","Blue","Green" };

        }

        private void CancelCreateAppointment()
        {
            CoreMethods.PopPageModel();
        }

        private async Task CreateNewAppointment()
        {
            if (Description!="")
            {
              //  var Appointment = CreateAppointment(Description, Color.FromHex("#FFA2C139"), new DateTime(Date.Year, Date.Month, Date.Day, AppointmentStartTimeSpan.Hours, AppointmentStartTimeSpan.Minutes, 0), new DateTime(Date.Year, Date.Month, Date.Day, AppointmentEndTimeSpan.Hours, AppointmentEndTimeSpan.Minutes, 0));
              //  MessagingCenter.Send(this, "CreatedNewAppointment", Appointment);
                    using (var client = new HttpClient())
                    {
                        try
                        {
                            var encoded = JWTService.Encode(new Dictionary<string, object>
                             {
                                { "subject",Description},
                                { "color", "#FFA2C139"},
                                { "startTime",new DateTime(Date.Year, Date.Month, Date.Day, AppointmentStartTimeSpan.Hours, AppointmentStartTimeSpan.Minutes, 0) },
                                { "endTime", new DateTime(Date.Year, Date.Month, Date.Day, AppointmentEndTimeSpan.Hours, AppointmentEndTimeSpan.Minutes, 0) }
                            });
                        var values = new Dictionary<string, string> { { "content", encoded } };
                        var content = new FormUrlEncodedContent(values);
                        var result = await client.PutAsync(new Uri("http://dementiahelper.azurewebsites.net/api/values/createnewappointment"), content);
                        var decoded = JWTService.Decode(await result.Content.ReadAsStringAsync());
                        await CoreMethods.PopPageModel();
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
            }
        
        private bool AppointmentCreated(Dictionary<string, object> dict)
        {
            return (bool)dict["AppointmentCreated"];
        }

        public ScheduleAppointment CreateAppointment(string subject, Color color, DateTime startTime, DateTime endTime)
        {
            ScheduleAppointment scheduleAppointment = new ScheduleAppointment() { Subject = subject, Color = color, StartTime = startTime, EndTime = endTime };
            return scheduleAppointment;

        }

    }
}

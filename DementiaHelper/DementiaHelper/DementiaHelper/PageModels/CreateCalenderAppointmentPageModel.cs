using DementiaHelper.Pages;
using Syncfusion.SfSchedule.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
            this.CreateAppointmentCommand = new Command( () =>  CreateNewAppointment());
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

        private void CreateNewAppointment()
        {
            if (Description!="")
            {
                var Appointment = CreateAppointment(Description, Color.FromHex("#FFA2C139"), new DateTime(Date.Year, Date.Month, Date.Day, AppointmentStartTimeSpan.Hours, AppointmentStartTimeSpan.Minutes, 0), new DateTime(Date.Year, Date.Month, Date.Day, AppointmentEndTimeSpan.Hours, AppointmentEndTimeSpan.Minutes, 0));
                MessagingCenter.Send(this, "CreatedNewAppointment", Appointment);
               CoreMethods.PopPageModel();
            }
        }

        public ScheduleAppointment CreateAppointment(string subject, Color color, DateTime startTime, DateTime endTime)
        {
            ScheduleAppointment scheduleAppointment = new ScheduleAppointment() { Subject = subject, Color = color, StartTime = startTime, EndTime = endTime };
            return scheduleAppointment;

        }

    }
}

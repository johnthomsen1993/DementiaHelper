using Syncfusion.SfSchedule.XForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DementiaHelper.PageModels
{
   public class CitizenHomePageModel :FreshMvvm.FreshBasePageModel
    {
        private DateTime _currentTime { get; set; }
        public DateTime CurrentTime { get { return _currentTime; } set {
                _currentTime = value;
                RaisePropertyChanged("CurrentTime");
            }
        }

        private ScheduleAppointment _appointment { get; set; }
        public ScheduleAppointment Appointment { get { return _appointment; } set { _appointment = value;
                RaisePropertyChanged("Appointment");
            }
        }

        public CitizenHomePageModel()
        {
            Appointment = GetNextAppointment();
            Device.StartTimer(TimeSpan.FromSeconds(1), () => {
                Device.BeginInvokeOnMainThread(() => CurrentTime = DateTime.Now);
                return true;
            });
        }

        private ScheduleAppointment GetNextAppointment() {
            return new ScheduleAppointment() {Subject="Ole (Barnebarn) kommer til frokost",StartTime= new DateTime(2017,4,18,10,15,0), EndTime= new DateTime(2017, 4, 18, 12, 15, 0) };
        }
        private string GetCorrectLanguageWeekDay()
        {
            return "";
        }

    }
}

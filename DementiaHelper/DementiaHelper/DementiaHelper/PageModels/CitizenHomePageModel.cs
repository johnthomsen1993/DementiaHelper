using PropertyChanged;
using Syncfusion.SfSchedule.XForms;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DementiaHelper.PageModels
{
    [ImplementPropertyChanged]
    public class CitizenHomePageModel :FreshMvvm.FreshBasePageModel
    {
     
        public DateTime CurrentTime { get; set; }
       
        public string Weekday { get; set; }
        public string Month { get; set; }

        public ImageSource PictureOfWhoIsVisiting { get; set; }
        public ScheduleAppointment Appointment { get; set; }
        

        public CitizenHomePageModel()
        {
            var test = DateTime.Now;
            Weekday = FirstLetterToUpper(test.ToString("dddd", new CultureInfo("da-DK")));
            Month = FirstLetterToUpper( test.ToString("MMMM", new CultureInfo("da-DK")));
            PictureOfWhoIsVisiting = ImageSource.FromFile("FakeProfilBillede.png");
            Appointment = GetNextAppointment();
            Device.StartTimer(TimeSpan.FromSeconds(1), () => {
                    CurrentTime = DateTime.Now;
                    return true;
                });
        }
        private string FirstLetterToUpper(string str)  // made this one since CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower()); does not exist in xamarin.forms pcl yet, which would have been the proper way to do it
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }

        private ScheduleAppointment GetNextAppointment() {
            return new ScheduleAppointment() {Subject="Ole (Barnebarn) kommer til frokost",StartTime= new DateTime(2017,4,18,10,15,0), EndTime= new DateTime(2017, 4, 18, 12, 15, 0) };
        }
    }
}

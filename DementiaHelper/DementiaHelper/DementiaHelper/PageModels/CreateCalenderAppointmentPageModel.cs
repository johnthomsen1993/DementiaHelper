using DementiaHelper.Model;
using DementiaHelper.Pages;
using DementiaHelper.Services;
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
using DementiaHelper.Resx;
using Xamarin.Forms;

namespace DementiaHelper.PageModels
{
    [ImplementPropertyChanged]
    public class CreateCalenderAppointmentPageModel : FreshMvvm.FreshBasePageModel
    {
        private int CitizenId { get; set; }
        public ICommand CreateAppointmentCommand { get; protected set; }
        public ICommand CancelCreateAppointmentCommand { get; protected set; }
        public string Description { get; set; }
        public string SelecteColorName { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan AppointmentStartTimeSpan { get; set; }
        public TimeSpan AppointmentEndTimeSpan { get; set; } 
        public ObservableCollection<string> ColorList{ get; set; } 
        public CreateCalenderAppointmentPageModel()
        {
            this.CreateAppointmentCommand = new Command( async() => await CreateNewAppointment());
            this.CancelCreateAppointmentCommand = new Command(async () => await CoreMethods.PopPageModel());
            AppointmentStartTimeSpan = new TimeSpan(12, 0, 0);
            AppointmentEndTimeSpan = new TimeSpan(13, 0, 0);
            Description = "";
            Date = DateTime.Now;
            ColorList = new ObservableCollection<string>() {AppResources.Red,AppResources.Blue,AppResources.Green };

        }

        public override void Init(object initData)
        {
            base.Init(initData);
            CitizenId = (int)initData;
        }

        private async Task CreateNewAppointment()
        {
            if (Description!="")
            {
                var decoded = await ModelAccessor.Instance.CalendarController.CreateNewAppointment(SelecteColorName, Description, CitizenId,
                            new DateTime(Date.Year, Date.Month, Date.Day, AppointmentEndTimeSpan.Hours, AppointmentEndTimeSpan.Minutes, 0).ToUniversalTime(),
                            new DateTime(Date.Year, Date.Month, Date.Day, AppointmentStartTimeSpan.Hours, AppointmentStartTimeSpan.Minutes, 0).ToUniversalTime());
               await CoreMethods.PopPageModel();
            }
        }
    }
}

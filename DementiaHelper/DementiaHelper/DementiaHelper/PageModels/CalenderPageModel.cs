using DementiaHelper.Model;
using DementiaHelper.Pages;
using DementiaHelper.Resx;
using PropertyChanged;
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
    [ImplementPropertyChanged]
    public class CalenderPageModel : FreshMvvm.FreshBasePageModel
    {

        public ICommand AddAppointmentCommand { get; protected set; }
        public bool CreatedNewItem { get; set; }
        #region Properties

        public ObservableCollection<ScheduleAppointment> Appointments { get; set; }
        public ObservableCollection<ScheduleAppointment> CopyOfAppointments { get; set; }

        public override void Init(object initData)
        {
            base.Init(initData);
            CreatedNewItem = false;
            CopyOfAppointments = new ObservableCollection<ScheduleAppointment>();
            Appointments = new ObservableCollection<ScheduleAppointment>();
            MessagingCenter.Subscribe<CreateCalenderAppointmentPageModel, ScheduleAppointment>(this, "CreatedNewAppointment", (sender, arg) => {
                Appointments.Add(arg);
                CopyOfAppointments.Add(arg);
                CreatedNewItem = true;
            });
        }
        

        #endregion Properties

        #region Constructor
        public CalenderPageModel()
        {
            this.AddAppointmentCommand = new Command(async () => await GoToCreateAppointment());
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            Appointments = new ObservableCollection<ScheduleAppointment>();
                foreach (var Appointment in CopyOfAppointments)
                {
                    Appointments.Add(Appointment);
                }
        }
        protected override void ViewIsDisappearing(object sender, EventArgs e)
        {
            base.ViewIsDisappearing(sender, e);
            CreatedNewItem = false;
        }

        async Task GoToCreateAppointment()
        {
            await CoreMethods.PushPageModel<CreateCalenderAppointmentPageModel>();
        }
        #endregion Constructor



        public ScheduleAppointment CreateAppointment(string subject,Color color, DateTime startTime,DateTime endTime)
        {
            ScheduleAppointment scheduleAppointment = new ScheduleAppointment() {Subject=subject ,Color = color, StartTime=startTime, EndTime = endTime };
            return scheduleAppointment;
        }
    }
}

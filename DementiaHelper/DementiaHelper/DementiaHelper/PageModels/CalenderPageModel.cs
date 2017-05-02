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
    public class CalenderPageModel : FreshMvvm.FreshBasePageModel
    {
        public const string URI_BASE = "http://dementiahelper.azurewebsites.net/api/values/calender/";
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
            //MessagingCenter.Subscribe<CreateCalenderAppointmentPageModel, ScheduleAppointment>(this, "CreatedNewAppointment", (sender, arg) => {
            //    Appointments.Add(arg);
            //    CopyOfAppointments.Add(arg);
            //    CreatedNewItem = true;
            //});
        }
        

        #endregion Properties

        #region Constructor
        public CalenderPageModel()
        {
            this.AddAppointmentCommand = new Command(async () => await GoToCreateAppointment());
            var User = (ApplicationUser)App.Current.Properties["ApplicationUser"];
            Device.BeginInvokeOnMainThread(async () =>
            {
                Appointments = await GetAppointments(User.CitizenId);
            });
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


        private async Task<ObservableCollection<ScheduleAppointment>> GetAppointments(int? id)
        {
            if (id == null) { return new ObservableCollection<ScheduleAppointment>();}
            using (var client = new HttpClient())
            {
                try
                {
                    var encoded = JWTService.Encode(new Dictionary<string, object>() { { "citizenId", id } });
                    var result = await client.GetStringAsync(new Uri(URI_BASE + encoded));
                    var decoded = JWTService.Decode(result);
                    return decoded.ContainsKey("CalenderAppointments") ? null : MapToCalenderAppointments(decoded);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        private ObservableCollection<ScheduleAppointment> MapToCalenderAppointments(IDictionary<string, object> decoded)
        {
            ObservableCollection<ScheduleAppointment> tempCalenderAppointmentsList = new ObservableCollection<ScheduleAppointment>();

            var list = decoded.SingleOrDefault(x => x.Key.Equals("CalenderAppointments")).Value as IEnumerable<object>;
            foreach (var obj in list)
            {
                var jsonContainer = obj as JContainer;
                tempCalenderAppointmentsList.Add(new ScheduleAppointment()
                {
                    Subject = jsonContainer.SelectToken("Subject").ToString(),
                    Color = Color.FromHex(jsonContainer.SelectToken("Color").ToString()),
                    StartTime = jsonContainer.SelectToken("StartTime").ToObject<DateTime>(),
                    EndTime = jsonContainer.SelectToken("EndTime").ToObject<DateTime>()
                });
            }
            return tempCalenderAppointmentsList;
        }
    

        async Task GoToCreateAppointment()
        {
            await CoreMethods.PushPageModel<CreateCalenderAppointmentPageModel>();
        }
        #endregion Constructor
    }
}

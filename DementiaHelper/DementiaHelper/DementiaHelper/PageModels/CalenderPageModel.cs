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
        #region Properties
        public const string URI_BASE = "http://dementiahelper.azurewebsites.net/api/values/calendar/";
        private ApplicationUser user = (ApplicationUser)App.Current.Properties["ApplicationUser"];
        public ICommand AddAppointmentCommand { get; protected set; }
        public bool RedrawView { get; set; }
        public ObservableCollection<ScheduleAppointment> Appointments { get; set; }
        #endregion Properties
        public override void Init(object initData)
        {
            base.Init(initData);
            RedrawView = true;
      
            Device.BeginInvokeOnMainThread(async () =>
            {
                Appointments = await GetAppointments(user.CitizenId);
            });
        }
        


        #region Constructor
        public CalenderPageModel()
        {
            this.AddAppointmentCommand = new Command(async (id) => await GoToCreateAppointment(user.CitizenId));
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            Device.BeginInvokeOnMainThread(async () =>
            {
                Appointments = await GetAppointments(user.CitizenId);
            });
        }
        protected override void ViewIsDisappearing(object sender, EventArgs e)
        {
            base.ViewIsDisappearing(sender, e);
            
        }


        private async Task<ObservableCollection<ScheduleAppointment>> GetAppointments(int? id)
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

        private ObservableCollection<ScheduleAppointment> MapToCalenderAppointments(IDictionary<string, object> decoded)
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
    

        async Task GoToCreateAppointment(int? id)
        {
            await CoreMethods.PushPageModel<CreateCalenderAppointmentPageModel>(id);
        }
        #endregion Constructor
    }
}

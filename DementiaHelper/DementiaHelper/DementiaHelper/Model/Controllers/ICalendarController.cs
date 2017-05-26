using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.SfSchedule.XForms;

namespace DementiaHelper.Model.Controllers
{
    public interface ICalendarController
    {
        ObservableCollection<ScheduleAppointment> MapToCalenderAppointments(IDictionary<string, object> decoded);
        Task<ObservableCollection<ScheduleAppointment>> GetAppointments(int? id);
        Task<ScheduleAppointment> GetAppointment();
        ScheduleAppointment MapToCalenderAppointment(IDictionary<string, object> decoded);

        Task<IDictionary<string, object>> CreateNewAppointment(string selecteColorName, string description,int citizenId, DateTime endTime, DateTime startTime);
        string GetHexColor(string selecteColorName);
    }
}

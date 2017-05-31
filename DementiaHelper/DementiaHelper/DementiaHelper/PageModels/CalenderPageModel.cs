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
    public class CalendarPageModel : FreshMvvm.FreshBasePageModel
    {
        #region ViewModel Properties
        public ICommand GoToDayViewCommand { get; protected set; }
        public ICommand GoToWeekViewCommand { get; protected set; }
        public ICommand GoToMonthViewCommand { get; protected set; }
        #endregion

        public CalendarPageModel()
        {
            this.GoToDayViewCommand = new Command(async () => await GoToDayView());
            this.GoToWeekViewCommand = new Command(async () => await GoToWeekView());
            this.GoToMonthViewCommand = new Command(async () => await GoToMonthView());
        }

        private async Task GoToMonthView()
        {
            await CoreMethods.PushPageModel<MonthCalendarPageModel>();
        }

        private async Task GoToWeekView()
        {
            await CoreMethods.PushPageModel<WeekCalendarPageModel>();
        }

        private async Task GoToDayView()
        {
            await CoreMethods.PushPageModel<DayCalendarPageModel>();
        }
    }
}

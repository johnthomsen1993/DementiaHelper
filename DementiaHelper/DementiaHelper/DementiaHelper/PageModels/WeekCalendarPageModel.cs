﻿using DementiaHelper.Model;
using Syncfusion.SfSchedule.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DementiaHelper.Resx;
using Xamarin.Forms;

namespace DementiaHelper.PageModels
{
    public class WeekCalendarPageModel : FreshMvvm.FreshBasePageModel
    {
        #region Properties
        public ICommand AddAppointmentCommand { get; protected set; }
        public ObservableCollection<ScheduleAppointment> Appointments { get; set; }
        #endregion Properties
        public WeekCalendarPageModel()
        {
            this.AddAppointmentCommand = new Command(async () => await GoToCreateAppointment());
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            Device.BeginInvokeOnMainThread(async () =>
            {
                Appointments = await ModelAccessor.Instance.CalendarController.GetAppointments(((ApplicationUser)App.Current.Properties["ApplicationUser"]).CitizenId);
            });
        }
        async Task GoToCreateAppointment()
        {
            if (((ApplicationUser)App.Current.Properties["ApplicationUser"]).CitizenId != null)
            {
                await CoreMethods.PushPageModel<CreateCalenderAppointmentPageModel>(((ApplicationUser)App.Current.Properties["ApplicationUser"]).CitizenId);
            }
            else
            {
                switch (((ApplicationUser)App.Current.Properties["ApplicationUser"]).RoleId)
                {
                    case 2:
                        await CoreMethods.DisplayAlert(AppResources.RelativeAddFailureTitle,
                            AppResources.CalendarAddRelativeFailureSubjekt, AppResources.General_Ok);
                        break;
                    case 3:
                        await CoreMethods.DisplayAlert(AppResources.CaregiverAddFailureTitle,
                            AppResources.CalendarAddCaregiverFailureSubjekt, AppResources.General_Ok);
                        break;
                }
            }
        }
    }
}

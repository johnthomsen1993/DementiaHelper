using DementiaHelper.PageModels;
using DementiaHelper.Resx;
using DementiaHelper.Services;
using Syncfusion.SfSchedule.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace DementiaHelper.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalenderPage : TabbedPage
    {
        // Using a DependencyProperty as the backing store for MyString. This enables animation, styling, binding, etc...
        public static readonly BindableProperty AppointmentsProperty =
          BindableProperty.Create("AppointmentsProperty", typeof(ObservableCollection<ScheduleAppointment>), typeof(CalenderPage), new ObservableCollection<ScheduleAppointment>());
        public ObservableCollection<ScheduleAppointment> Appointments
        {
            get { return (ObservableCollection<ScheduleAppointment>)GetValue(AppointmentsProperty); }
            set { SetValue(AppointmentsProperty, value); }

        }

        public CalenderPage()
        {
           SetBinding(AppointmentsProperty, new Binding("Appointments"));
           
            InitializeComponent();
           
         //   DaySchedule.DataSource = Appointments;
            //     WeekSchedule.SetBinding(SfSchedule.DataSourceProperty, new Binding("Appointments"));
           // MonthSchedule.DataSource = Appointments;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            
   
            MessagingCenter.Subscribe<CalenderPageModel>(this, "New Appointments", (sender) => {
                UpdateChildrenLayout();
            });
            //SetBinding(AppointmentsProperty, new Binding("Appointments"));

            //    InitializeComponent();
            //DaySchedule.DataSource = Appointments;
            //     WeekSchedule.SetBinding(SfSchedule.DataSourceProperty, new Binding("Appointments"));
            //MonthSchedule.DataSource = Appointments;
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            CurrentPage.BindingContext = this.BindingContext;
            DaySchedule.DataSource = Appointments;
            MonthSchedule.DataSource = Appointments;

            this.UpdateChildrenLayout();
           

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<CalenderPageModel>(this, "New Appointments");
        }
        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () => {
                var result = await this.DisplayAlert(AppResources.Warning + "!", AppResources.AreYouSureThatYouWantToCloseThisApplication, AppResources.YesText, AppResources.NoText);
                if (result)
                {

                    INativeService nativeHelper = DependencyService.Get<INativeService>();
                    if (nativeHelper != null)
                    {
                        nativeHelper.CloseApp();
                    }
                }
            });

            return true;
        }
    }
}

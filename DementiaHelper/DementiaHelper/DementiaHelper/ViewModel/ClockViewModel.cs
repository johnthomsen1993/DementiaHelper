
using System;
using System.ComponentModel;
using System.Windows.Input;
using DementiaHelper.Extensions;
using Xamarin.Forms;

namespace DementiaHelper.ViewModel
{
    public class ClockViewModel : BaseViewModel
    {
        DateTime dateTime;

        public ClockViewModel()
        {
            this.DateTime = DateTime.Now;

            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                this.DateTime = DateTime.Now;
                return true;
            });
        }

        public DateTime DateTime
        {
            set
            {
                if (dateTime != value)
                {
                    dateTime = value;
                }
            }
            get
            {
                return dateTime;
            }
        }
    }
}

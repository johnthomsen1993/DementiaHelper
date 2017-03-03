using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DementiaHelper.Extensions;
using DementiaHelper.Model;
using DementiaHelper.Services;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace DementiaHelper.ViewModel
{
    class EditAccountInformationViewModel : BaseViewModel
    {
        public User User { get; set; }
        public ICommand SaveCommand { get; protected set; }
        public ICommand CancelCommand { get; protected set; }

        public EditAccountInformationViewModel(User user)
        {
            User = user;
            this.SaveCommand = new Command(async () => await Save());
            this.CancelCommand = new Command(async () => await Cancel());
        }
        async Task Save()
        {
            await NavigationService.PopModalAsync();
        }
        async Task Cancel()
        {
            await NavigationService.PopModalAsync();
        }

        
    }
}

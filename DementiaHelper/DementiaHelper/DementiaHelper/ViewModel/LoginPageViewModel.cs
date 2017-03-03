using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DementiaHelper.Extensions;
using System.Windows.Input;
using DementiaHelper.Services;
using Xamarin.Forms;

namespace DementiaHelper.ViewModel
{
   
   public class LoginPageViewModel : BaseViewModel
    {
      public ICommand LoginCommand { get; protected set; }
      public ICommand GoToCreateAccountCommand { get; protected set; }
      //public ICommand GoToAccountInformationCommand { get; protected set; }

        public LoginPageViewModel()
      {
          this.LoginCommand = new Command(async () => await LoginAsync());
          this.GoToCreateAccountCommand = new Command(async () => await GoToCreateAccount());
          //this.GoToAccountInformationCommand = new Command(async () => await GoToAccountInformation());

        }

        async Task LoginAsync()
        {
           await NavigationService.PushAsync(new ClockViewModel());
        }
        async Task GoToCreateAccount()
        {
            await NavigationService.PushModalAsync(new CreateAccountViewModel());
        }
        //async Task GoToAccountInformation()
        //{
        //    await NavigationService.PushModalAsync(new AccountInformationViewModel());
        //}
    }
}

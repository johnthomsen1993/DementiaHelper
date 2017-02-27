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

      public LoginPageViewModel()
      {
          this.LoginCommand = new Command(async () => await LoginAsync());
     
        }

        async Task LoginAsync()
        {
           await NavigationService.PushAsync(new ClockViewModel());
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DementiaHelper.Extensions;
using DementiaHelper.Model;
using DementiaHelper.Services;
using Xamarin.Forms;

namespace DementiaHelper.ViewModel
{
    public class AccountInformationViewModel: BaseViewModel
    {
        public User User { get; set; }
        public bool EditButton { get; set; }
        public ICommand GoToEditAccountInformationCommand { get; protected set; }
        public ICommand BackCommand { get; protected set; }

        public AccountInformationViewModel()
        {
            User = new User() {Id = "test@email.com", FirstName = "Claus", Email = "test@email.com", Description = "This is a default in the viewmodel"};

            EditButton = User.Id == "test@email.com";
            this.GoToEditAccountInformationCommand = new Command(async () => await GoToEditAccountInformation());
            BackCommand = new Command(async () => await Back());
        }

        async Task GoToEditAccountInformation()
        {
            await NavigationService.PushModalAsync(new EditAccountInformationViewModel(User));
        }
        async Task Back()
        {
            await NavigationService.PopModalAsync();
        }
    }
}

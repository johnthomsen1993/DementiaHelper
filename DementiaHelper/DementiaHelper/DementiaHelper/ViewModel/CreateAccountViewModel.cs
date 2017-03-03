using System.Threading.Tasks;
using System.Windows.Input;
using DementiaHelper.Extensions;
using DementiaHelper.Model;
using DementiaHelper.Services;
using Xamarin.Forms;

namespace DementiaHelper.ViewModel
{
    public class CreateAccountViewModel : BaseViewModel
    {
        public User User { get; set; }
        public ICommand CancelCreateAccountCommand { get; protected set; }
        public ICommand CreateAccountCommand { get; protected set; }
        public CreateAccountViewModel()
        {
            User = new User();
            this.CancelCreateAccountCommand = new Command(async () => await NavigationService.PopModalAsync());
            this.CreateAccountCommand = new Command<User>((User) =>
            {
             //  App.Current.MainPage.DisplayAlert(User.Name, "Test", "OK");
               DependencyService.Get<ICredentialsService>().SaveCredentials(User.FirstName,"password");

            });
         
        }

    }
}
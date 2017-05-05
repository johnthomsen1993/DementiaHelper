using System;
using System.Threading.Tasks;
using System.Windows.Input;
using DementiaHelper.Extensions;
using DementiaHelper.Model;
using DementiaHelper.Services;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Net.Http;
using System.Collections.ObjectModel;
using DementiaHelper.Resx;
using PropertyChanged;
using System.Text.RegularExpressions;

namespace DementiaHelper.PageModels
{
    [ImplementPropertyChanged]
    public class CreateAccountPageModel : FreshMvvm.FreshBasePageModel
    {
        public ObservableCollection<string> RoleCollection { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ICommand CancelCreateAccountCommand { get; protected set; }
        public ICommand CreateAccountCommand { get; protected set; }
        public CreateAccountPageModel()
        {

            this.CancelCreateAccountCommand = new Command(async () => await CoreMethods.PopPageModel());
            this.CreateAccountCommand = new Command(async () => await CreateAccountAsync());
            this.RoleCollection = new ObservableCollection<string>
        {
            {AppResources.DementiaRole}, {AppResources.CaregiverRole},{AppResources.RelativesRole}

        };
        }


        public string SelecteRoleName { get; set; }

        async Task CreateAccountAsync()
        {
            var RoleId = GetIntegerValueForDatabase();
            if (RoleId == 0)
            {
                await CoreMethods.DisplayAlert(AppResources.Account_InvalidRoleTitle, AppResources.Account_InvalidRoleText, AppResources.General_Ok);
                return; 
            }
            if (!IsValidEmail(Email))
            {
                await CoreMethods.DisplayAlert(AppResources.General_InvalidEmailTitle, AppResources.General_InvalidEmailText, AppResources.General_Ok);
                return;
            }
            if (Password.Length < 6)
            {
                await CoreMethods.DisplayAlert(AppResources.Account_InvalidPasswordTitle, AppResources.Account_InvalidPasswordText, AppResources.General_Ok);
                return;
            }
            using (var client = new HttpClient())
            {
                var encoded = JWTService.Encode(new Dictionary<string, object>
            {
                {"email",Email},
                {"password", Password},
                {"role", RoleId},
                {"firstName", FirstName},
                {"lastName", LastName}
            });
                var values = new Dictionary<string, string> { { "token", encoded } };
                var content = new FormUrlEncodedContent(values);
                var result = await client.PutAsync(new Uri("http://dementiahelper.azurewebsites.net/api/account/createaccount"), content);
                var decoded = JWTService.Decode(await result.Content.ReadAsStringAsync());
                if (decoded != null)
                {
                    if (UserCreated(decoded))
                    {
                        if (await App.LoginAsync(Email, Password))
                        {
                            App.SetMasterDetailToRole();
                            CoreMethods.SwitchOutRootNavigation(App.NavigationStacks.MainAppStack);
                            await CoreMethods.PopPageModel();
                        }
                    }
                }
                else
                {
                   await CoreMethods.DisplayAlert("Fejl", "Det var ikke muligt at forbinde til vores server prøv igen senere", "Ok");
                }
            };
        }

        private bool IsValidEmail(string inputEmail)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }

        private bool UserCreated(IDictionary<string, object> dict)
        {
            return (bool)dict["UserCreated"];
        }
        public int GetIntegerValueForDatabase()
        {
            if (SelecteRoleName == AppResources.DementiaRole)
            {
                return 1;
            }
            else if (SelecteRoleName == AppResources.RelativesRole)
            {
                return 2;
            }
            else if (SelecteRoleName == AppResources.CaregiverRole)
            {
                return 3;
            }
            else { return 0; }
        }
    }


}
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

namespace DementiaHelper.PageModels
{
    public class CreateAccountPageModel : FreshMvvm.FreshBasePageModel
    {
        public ObservableCollection<string> RoleCollection { get; set; }
        public UserInformation User { get; set; }
        public string Password { get; set; }
        public ICommand CancelCreateAccountCommand { get; protected set; }
        public ICommand CreateAccountCommand { get; protected set; }
        public CreateAccountPageModel()
        {
            User = new UserInformation();
            this.CancelCreateAccountCommand = new Command(async () => await CoreMethods.PopPageModel());
            this.CreateAccountCommand = new Command( async() => await CreateAccountAsync());
            this.RoleCollection = new ObservableCollection<string>
        {
            {AppResources.DementiaRole}, {AppResources.CaregiverRole},{AppResources.RelativesRole}

        };
        }
        private string _selectedRoleName;
        public string SelecteRoleName
        {
            get { return _selectedRoleName; }
            set
            {
                if (_selectedRoleName != value)
                {
                    _selectedRoleName = value;
                    RaisePropertyChanged("SelecteRoleName");
                }
            }
        }

        async Task CreateAccountAsync()
        {
            var values = new Dictionary<string, string>
            {
                {"email",User.Email},
                {"password", Password}
            };
            using (var h = new HttpClient())
            {
                var content = new FormUrlEncodedContent(values);
                var result = h.PostAsync(new Uri("http://dementiahelper.azurewebsites.net/api/account/createaccount"), content).Result;
                var response = result.Content.ReadAsStringAsync();
                await App.Current.MainPage.DisplayAlert(response.Result, "Test", "OK");
            }
        }

    }
}
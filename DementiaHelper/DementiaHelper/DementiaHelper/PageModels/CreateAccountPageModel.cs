﻿using System;
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

namespace DementiaHelper.PageModels
{
    [ImplementPropertyChanged]
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


        public string SelecteRoleName { get; set; }

        async Task CreateAccountAsync()
        {
            var values = new Dictionary<string, object>
            {
                {"email",User.Email},
                {"password", Password},
                {"role", SelecteRoleName }
            };
            //using (var h = new HttpClient())
            //{
            //    var content = new FormUrlEncodedContent(values);
            //    var result = h.PostAsync(new Uri("http://dementiahelper.azurewebsites.net/api/account/createaccount"), content).Result;
            //    var response = result.Content.ReadAsStringAsync();
            //    await App.Current.MainPage.DisplayAlert(response.Result, "Test", "OK");
            //}
            App.SetMasterDetailToRole();
            CoreMethods.SwitchOutRootNavigation(App.NavigationStacks.MainAppStack);
        }

    }
}
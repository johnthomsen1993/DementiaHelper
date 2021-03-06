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
using System.Text.RegularExpressions;

namespace DementiaHelper.PageModels
{
    [ImplementPropertyChanged]
    public class CreateAccountPageModel : FreshMvvm.FreshBasePageModel
    {
        #region ViewModel Properties
        public ObservableCollection<string> RoleCollection { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string SelecteRoleName { get; set; }
        public ICommand CancelCreateAccountCommand { get; protected set; }
        public ICommand CreateAccountCommand { get; protected set; }
        #endregion
        public CreateAccountPageModel()
        {

            this.CancelCreateAccountCommand = new Command(async () => await CoreMethods.PopPageModel());
            this.CreateAccountCommand = new Command(async () => await CreateAccountAsync());
            this.RoleCollection = new ObservableCollection<string>
            {
                {AppResources.DementiaRole}, {AppResources.CaregiverRole},{AppResources.RelativesRole}

            };
        }
        async Task CreateAccountAsync()
        {
            if (FirstName == null || LastName == null || Password == null || Email == null || FirstName == "" || LastName == "")
            {
                await CoreMethods.DisplayAlert(AppResources.General_NullTitle, AppResources.General_NullText, AppResources.General_Ok);
                return;
            }
            var RoleId = GetRoleIntegerValue();
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

                var decoded = await ModelAccessor.Instance.AccountController.CreateAccount(Email, Password, RoleId, FirstName, LastName);
                if (decoded != null)
                {
                    if ((bool)decoded["UserCreated"])
                    {
                        if (await ModelAccessor.Instance.AccountController.LoginAsync(Email, Password) == 1)
                        {
                            App.SetMasterDetailToRole();
                            CoreMethods.SwitchOutRootNavigation(App.NavigationStacks.MainAppStack);
                            await CoreMethods.PopPageModel();
                        }
                    }
                }
                else
                {
                   await CoreMethods.DisplayAlert(AppResources.Connection_ErrorTitle, AppResources.Connection_ErrorText, AppResources.General_Ok);
                }
           
        }

        private bool IsValidEmail(string inputEmail)
        {
            if (inputEmail == null) return false;
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }


        public int GetRoleIntegerValue()
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
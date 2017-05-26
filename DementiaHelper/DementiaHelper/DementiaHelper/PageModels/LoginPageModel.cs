﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DementiaHelper.Extensions;
using System.Windows.Input;
using DementiaHelper.Services;
using DementiaHelper.PageModels;
using Xamarin.Forms;
using DementiaHelper.Pages;
using System.Net.Http.Headers;
using DementiaHelper.Model;
using DementiaHelper.Resx;
using Newtonsoft.Json.Linq;

namespace DementiaHelper.PageModels
{
   
   public class LoginPageModel : FreshMvvm.FreshBasePageModel
    {

    
      public string Email { get; set; }
      public string Password { get; set; }
      public ICommand LoginCommand { get; protected set; }
      public ICommand GoToCreateAccountCommand { get; protected set; }
        public LoginPageModel()
        {
            Email = "";
            Password = "";
        }
        public override void Init(object initData)
        {
            base.Init(initData);
            this.LoginCommand = new Command( async() => await LoginAsync());
            this.GoToCreateAccountCommand = new Command(async () => await GoToCreateAccount());
        }

        async Task LoginAsync()
        {
            if (await ModelAccessor.Instance.AccountController.LoginAsync(Email, Password))
            {
                Email = "";
                Password = "";
                App.SetMasterDetailToRole();
                CoreMethods.SwitchOutRootNavigation(App.NavigationStacks.MainAppStack);

            }else
            {
                await CoreMethods.DisplayAlert(AppResources.Account_LoginFailTitle,AppResources.Account_LoginFailText,AppResources.General_Ok);
            }

        }
        async Task GoToCreateAccount()
        {
            await CoreMethods.PushPageModel<CreateAccountPageModel>();
        }
    }
}

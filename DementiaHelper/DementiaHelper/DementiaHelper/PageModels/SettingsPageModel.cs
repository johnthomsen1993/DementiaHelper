
using System;
using System.ComponentModel;
using System.Windows.Input;
using DementiaHelper.Extensions;
using Xamarin.Forms;
using DementiaHelper.Services;

namespace DementiaHelper.PageModels
{
    public class SettingsPageModel : FreshMvvm.FreshBasePageModel
    {
        public ICommand LogOutCommand { get; protected set; }
        public SettingsPageModel()
        {
            LogOutCommand = new Command( () => LogOut());
        }
        public void LogOut()
        {
            DependencyService.Get<ICredentialsService>().DeleteCredentials();
            CoreMethods.SwitchOutRootNavigation(App.NavigationStacks.LoginNavigationStack);
        }
    }
}

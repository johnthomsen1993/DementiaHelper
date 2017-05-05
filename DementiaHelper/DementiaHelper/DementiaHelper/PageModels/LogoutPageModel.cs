
using System;
using System.ComponentModel;
using System.Windows.Input;
using DementiaHelper.Extensions;
using Xamarin.Forms;
using DementiaHelper.Services;

namespace DementiaHelper.PageModels
{
    public class LogOutPageModel : FreshMvvm.FreshBasePageModel
    {
        public LogOutPageModel()
        {
        }
        public void LogOut()
        {
            DependencyService.Get<ICredentialsService>().DeleteCredentials();
            App.SetLoginPageContainer();
            CoreMethods.SwitchOutRootNavigation(App.NavigationStacks.LoginNavigationStack);
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            Device.BeginInvokeOnMainThread( () =>
            {
                LogOut();
            });
        }
    }
}

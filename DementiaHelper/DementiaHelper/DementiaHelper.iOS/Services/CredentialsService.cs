using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using DementiaHelper.Services;
using Xamarin.Auth;
using System.Threading.Tasks;
using DementiaHelper.iOS.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(CredentialsService))]
namespace DementiaHelper.iOS.Services
{
    public class CredentialsService : ICredentialsService
    {
        public CredentialsService()
        {

        }
        public bool Authenticate()
        {
            var account = AccountStore.Create().FindAccountsForService(App.AppName).FirstOrDefault();
            if (account != null && account.Username != null && account.Properties["Password"] != null)
            {
                if ( App.Login(account.Username, account.Properties["Password"]))
                {
                    return true;
                }
            }
            return false;
        }

        public void SaveCredentials(string userName, string password)
        {
            if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(password))
            {
                Account account = new Account
                {
                    Username = userName
                };
                account.Properties.Add("Password", password);
                AccountStore.Create().Save(account, App.AppName);
            }

        }

        public void DeleteCredentials()
        {
            var account = AccountStore.Create().FindAccountsForService(App.AppName).FirstOrDefault();
            if (account != null)
            {
                AccountStore.Create().Delete(account, App.AppName);
            }
        }

        public bool DoCredentialsExist()
        {
            return AccountStore.Create().FindAccountsForService(App.AppName).Any() ? true : false;
        }

    
    }
}
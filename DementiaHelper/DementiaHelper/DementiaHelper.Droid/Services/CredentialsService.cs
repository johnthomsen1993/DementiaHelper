using System;
using System.Linq;
using DementiaHelper.Droid;
using DementiaHelper.Services;
using Xamarin.Auth;
using Xamarin.Forms;
using System.Threading.Tasks;
using DementiaHelper.Model;

[assembly: Dependency(typeof(CredentialsService))]
namespace DementiaHelper.Droid
{
    public class CredentialsService : ICredentialsService
    {
        public bool Authenticate()
        {
            var account = AccountStore.Create(Forms.Context).FindAccountsForService(App.AppName).FirstOrDefault();
            if (account != null && account.Username!=null && account.Properties["Password"]!=null )
            {
                    if ( ModelAccessor.Instance.AccountController.Login(account.Username, account.Properties["Password"]))
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
                AccountStore.Create(Forms.Context).Save(account, App.AppName);
            }
        }

        public void DeleteCredentials()
        {
            var account = AccountStore.Create(Forms.Context).FindAccountsForService(App.AppName).FirstOrDefault();
            if (account != null)
            {
                AccountStore.Create(Forms.Context).Delete(account, App.AppName);
            }
        }

        public bool DoCredentialsExist()
        {
            return AccountStore.Create(Forms.Context).FindAccountsForService(App.AppName).Any() ? true : false;
        }
    }
}
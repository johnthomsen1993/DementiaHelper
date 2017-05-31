using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DementiaHelper.Services
{

    /// <summary>
    /// Adapted from https://github.com/xamarin/recipes/blob/master/cross-platform/xamarin-forms/General/StoreCredentials/StoreCredentials/ICredentialsService.cs
    /// </summary>
    public interface ICredentialsService
    {
        bool Authenticate();

        void SaveCredentials(string userName, string password);

        void DeleteCredentials();

        bool DoCredentialsExist();
    }
}

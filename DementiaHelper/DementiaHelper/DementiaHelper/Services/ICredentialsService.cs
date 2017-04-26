using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DementiaHelper.Services
{
    public interface ICredentialsService
    {
        Task<bool> Authenticate();

        void SaveCredentials(string userName, string password);

        void DeleteCredentials();

        bool DoCredentialsExist();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DementiaHelper.WebApi.Data
{
    public interface IRepository
    {
        bool createAccount(string userName, string password);
        bool UpdateAccount(string id, string firstName, string lastName, string email, string description, byte[] picture);
        void GetAccount(string id);
    }
}

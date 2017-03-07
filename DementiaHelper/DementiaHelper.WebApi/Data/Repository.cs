using System.Collections.Generic;
using System.Threading.Tasks;

using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DementiaHelper.WebApi.Data
{
    public class Repository : IRepository
    {
        public bool createAccount(string userName, string password)
        {
            return true;
        }

        public bool UpdateAccount(string id, string firstName, string lastName, string email, string description, byte[] picture)
        {
            return true;
        }

        public void GetAccount(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}

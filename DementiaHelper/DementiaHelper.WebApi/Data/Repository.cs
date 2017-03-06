using System.Collections.Generic;
using System.Threading.Tasks;

using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DementiaHelper.WebApi.Data
{
    public class Repository : IRepository
    {
        public bool createAccount(string userName, string password)
        {
            return true;
        }
    }
}

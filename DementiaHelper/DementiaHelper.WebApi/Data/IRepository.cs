using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DementiaHelper.WebApi.model;

namespace DementiaHelper.WebApi.Data
{
    public interface IRepository
    {
        string CreateAccount(ApplicationUser user);
        ApplicationUser FetchApplicationUser(string email);
    }
}

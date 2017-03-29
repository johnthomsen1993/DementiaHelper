using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DementiaHelper.WebApi.model;

namespace DementiaHelper.WebApi.Data
{
    public interface IRepository
    {
        
        bool UpdateAccount(string firstName, string lastName, string email, string description, byte[] picture);
        AccountInformation GetAccount(string email);
        string CreateAccount(ApplicationUser user);
        ApplicationUser FetchApplicationUser(string email);
        ShoppingList GetShoppingList(string citizenId);
    }
}

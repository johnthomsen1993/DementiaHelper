using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DementiaHelper.WebApi.model;

namespace DementiaHelper.WebApi.Data
{
    public interface IRepository
    {
        void CreateAccountInformation(string firstName, string lastName, string email, string description);
        bool UpdateAccount(string firstName, string lastName, string email, string description);
        Dictionary<string, object> GetAccount(string email);
        string CreateAccount(ApplicationUser user);
        ApplicationUser FetchApplicationUser(string email);
        bool CheckIfUserExists(string email);
        List<ShoppingListDetail> GetShoppingList(int citizenId);
        bool SaveItemInShoppingList(int shoppinglistId, string item, int quantity);
    }
}

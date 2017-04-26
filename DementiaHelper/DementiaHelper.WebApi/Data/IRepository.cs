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
        bool CreateAccount(ApplicationUser user);
        ApplicationUser FetchApplicationUser(string email);
        bool CheckIfUserExists(string email);
        List<ShoppingListDetail> GetShoppingList(int citizenId);
        bool SaveItemInShoppingList(int shoppinglistId, string item, int quantity);
        bool RemoveShoppingListDetail(int id);
        Citizen GetCitizen(int id);
        Relative GetRelative(int id);
        Caregiver GetCaregiver(int id);
        RelativeConnection GetRelativeConnection(int id);
        CaregiverConnection GetCaregiverConnection(int id);
        List<CaregiverConnection> GetCaregiverConnections(int id);
        List<Appointment> GetAppointments(int id);
        void CreateAppointment(Appointment appointment);
    }
}

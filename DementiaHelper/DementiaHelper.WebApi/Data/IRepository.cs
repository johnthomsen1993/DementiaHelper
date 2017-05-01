using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DementiaHelper.WebApi.model;

namespace DementiaHelper.WebApi.Data
{
    public interface IRepository
    {
        bool UpdateAccount(ApplicationUser user);
        ApplicationUser GetApplicationUser(string email);
        bool CreateAccount(ApplicationUser user, string connectionId = null);
        ApplicationUser FetchApplicationUser(string email);
        bool CheckIfUserExists(string email);
        List<ShoppingListItem> GetShoppingList(int citizenId);
        bool SaveItemInShoppingList(int shoppinglistId, string item, int quantity);
        bool RemoveShoppingListItem(int id);
        Citizen GetCitizen(int id);
        List<Citizen> GetCitizenList(int id);
        Relative GetRelative(int id);
        Caregiver GetCaregiver(int id);
        List<Appointment> GetAppointments(int id);
        void CreateAppointment(Appointment appointment);
        void SaveChatMessage(string message, int group, string sender);
        void AddMemberToGroup(int group, string email);
        ICollection<ChatMessage> GetChatMessagesForGroup(int groupId);
        bool ConnectToCitizen(int relativeId, string connectionId);
        bool ConnectToCaregiver(int citizenId, string connectionId);
    }
}

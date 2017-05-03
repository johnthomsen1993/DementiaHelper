using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DementiaHelper.WebApi.model;

namespace DementiaHelper.WebApi.Data
{
    public interface IRepository
    {
        bool UpdateAccount(ApplicationUser user, string email);
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
        bool CitizenConnectToCaregiverCenter(int citizenId, string connectionId);
        bool CaregiverConnectToCaregiverCenter(int caregiverId, string connectionId);
        List<Relative> GetRelativesConnectedToId(int id);
        CaregiverCenter GetCaregiverCenterForCitizen(int id);
        bool DeleteAppointment(int id);
        void CreateNote(Note note);
        List<Note> GetNotes(int id);
        bool DeleteNote(int id);
    }
}

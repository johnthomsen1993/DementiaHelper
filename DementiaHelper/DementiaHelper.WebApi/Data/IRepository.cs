using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DementiaHelper.WebApi.model;

namespace DementiaHelper.WebApi.Data
{
    public interface IRepository
    {
        bool CreateAccount(ApplicationUser user, string connectionId = null);
        bool UpdateAccount(ApplicationUser user, string email);
        ApplicationUser GetApplicationUser(string email);
        ApplicationUser FetchApplicationUser(string email);
        Citizen GetCitizen(int id);
        List<Citizen> GetCitizenList(int id);
        Relative GetRelative(int id);
        Caregiver GetCaregiver(int id);
        bool SetNewPrimaryRelative(int citizenId, int newPrimaryRelative);
        bool CheckIfUserExists(string email);


        List<ShoppingListItem> GetShoppingList(int citizenId);
        bool SaveItemInShoppingList(int shoppinglistId, string item, int quantity);
        bool RemoveShoppingListItem(int id);
        bool ChangeBoughtStatus(int id, bool bought);


        List<Appointment> GetAppointments(int id);
        void CreateAppointment(Appointment appointment);
        bool DeleteAppointment(int id);
        Appointment GetLatestAppointment(int id);


        void SaveChatMessage(string message, int group, int sender);
        ChatGroup CreateChatGroup(string groupName, int groupRole);
        bool AddMemberToGroup(int group, int id);
        bool RemoveFromChatGroup(int group, int id);
        ICollection<ChatMessage> GetChatMessagesForGroup(int groupId);
        ICollection<ChatGroupConnection> GetChatgroupId(int id);


        Relative ConnectToCitizen(int relativeId, string connectionId);
        bool CitizenConnectToCaregiverCenter(int citizenId, string connectionId);
        bool CaregiverConnectToCaregiverCenter(int caregiverId, string connectionId);
        List<Relative> GetRelativesConnectedToId(int id);
        CaregiverCenter GetCaregiverCenterForCitizen(int id);


        void CreateNote(Note note);
        List<Note> GetNotes(int id);
        bool DeleteNote(int id);
        
    }
}

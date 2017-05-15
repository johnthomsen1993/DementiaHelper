using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using DementiaHelper.WebApi.model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Remotion.Linq.Clauses;

namespace DementiaHelper.WebApi.Data
{
    public class Repository : IRepository
    {
        private readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public bool UpdateAccount(ApplicationUser user, string email)
        {
            try
            {
                var target = _context.ApplicationUsers.SingleOrDefault(x => x.Email == email);

                target.FirstName = user.FirstName;
                target.LastName = user.LastName;
                target.Email = user.Email;
                target.Description = user.Description;
                target.Phone = user.Phone;

                _context.ApplicationUsers.Update(target);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("Exception were thrown when trying to update AccountInformation in database");
                return false;
            }
        }

        public ApplicationUser GetApplicationUser(string email)
        {
            try
            {
                return _context.ApplicationUsers.First(i=> i.Email == email);
            }
            catch (Exception)
            {
                Console.WriteLine("Exception were thrown when trying to get AccountInformation from database");
                return null;
            }
        }

        public bool CreateAccount(ApplicationUser user, string connectionId = null)
        {
            if (CheckIfUserExists(user.Email))
            {
                return false;
            }
            switch (user.RoleId)
            {
                case 1:
                    var citizen = new Citizen() {ApplicationUser = user, ConnectionId = connectionId};
                    _context.Add(citizen);
                    var chatGroup = CreateChatGroup(citizen.CitizenId.ToString(), 1);
                    AddMemberToGroup(chatGroup.ChatGroupId, user.ApplicationUserId);
                    CreateChatGroup(citizen.CitizenId.ToString(), 3);
                    _context.SaveChanges();
                    return true;
                case 2:
                    var relative = new Relative() {ApplicationUser = user, PrimaryRelative = false};
                    _context.Add(relative);
                    _context.SaveChanges();
                    return true;
                case 3:
                    var caregiver = new Caregiver() {ApplicationUser = user};
                    _context.Add(caregiver);
                    _context.SaveChanges();
                    return true;
                default:
                    return false;
            }
        }

        public ApplicationUser FetchApplicationUser(string email)
        {
            return _context.ApplicationUsers.Include(x => x.Role).SingleOrDefault(b => b.Email == email);
        }

        public bool CheckIfUserExists(string email)
        {
            return _context.ApplicationUsers.Any(information => information.Email.Equals(email));
        }

        public List<ShoppingListItem> GetShoppingList(int citizenId)
        {
            var queryable = _context.ShoppingListItems.AsQueryable();

            Expression<Func<ShoppingListItem, Product>> include_product = detail => detail.Product;

            return
                queryable.Where(
                    x => x.Citizen.CitizenId == citizenId).Include(include_product)
                    .ToList();
        }

        public bool SaveItemInShoppingList(int citizenId, string item, int quantity)
        {
            try
            {
                var query = from p in _context.Products
                            where p.ProductName.ToUpper().Trim() == item.ToUpper().Trim()
                            select p;

                Product product = query.SingleOrDefault();

                if (product == null)
                {
                    product = _context.Products.Add(new Product() {ProductName = item}).Entity;
                    _context.SaveChanges();
                }

                _context.ShoppingListItems.Add(new ShoppingListItem() {CitizenId = citizenId, Bought = false, Product = product, Quantity = quantity});
                _context.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public bool RemoveShoppingListItem(int id)
        {
            var item = _context.ShoppingListItems.SingleOrDefault(x => x.ShoppingListItemId == id);
            if (item == null) return false;
            _context.ShoppingListItems.Remove(item);
            _context.SaveChanges();
            return true;
        }

        public Citizen GetCitizen(int id)
        {
            return _context.Citizens.Include(x => x.ApplicationUser).SingleOrDefault(x => x.CitizenId == id);
        }

        public List<Citizen> GetCitizenList(int id)
        {
            return _context.Citizens.Include(x => x.ApplicationUser).Where(x => x.CaregiverCenterId == id).ToList();
        }

        public Relative GetRelative(int id)
        {
            return _context.Relatives.Include(x => x.ApplicationUser).SingleOrDefault(x => x.RelativeId == id);
        }

        public Caregiver GetCaregiver(int id)
        {
            return _context.Caregivers.Include(x => x.ApplicationUser).SingleOrDefault(x => x.CaregiverId == id);
        }

        public List<Appointment> GetAppointments(int id)
        {
            return _context.Appointments.Where(x => x.Citizen.CitizenId == id).ToList();
        }

        public void CreateAppointment(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            _context.SaveChanges();
        }

        public void SaveChatMessage(string message, int groupId, int sender)
        {
            _context.ChatMessages.Add(new ChatMessage()
            {
                ChatGroupId = groupId,
                Message = message,
                SenderId = sender
            });
            _context.SaveChanges();
        }

        public ChatGroup CreateChatGroup(string groupName, int groupRole)
        {
            var chatgroup = _context.ChatGroups.Add(new ChatGroup()
            {
                GroupName = groupName,
                GroupRole = groupRole
            });
            _context.SaveChanges();
            return chatgroup.Entity;
        }

        public bool AddMemberToGroup(int groupId, int userId)
        {
            _context.ChatGroupConnections.Add(new ChatGroupConnection()
            {
                ChatGroupId = groupId,
                ApplicationUserId = userId
            });
           _context.SaveChanges();
            return true;
        }

        public bool RemoveFromChatGroup(int groupId, int applicationUserId)
        {
            //ChatGroupConnection(){ChatGroupId = groupId,ApplicationUserId = applicationUserId}
            var connections = _context.ChatGroupConnections.Where(x => x.ChatGroupId == groupId && x.ApplicationUserId == applicationUserId);
            foreach (var connection in connections)
            {
                _context.ChatGroupConnections.Remove(connection);
            }
            _context.SaveChanges();
            return true;
        }

        public ICollection<ChatMessage> GetChatMessagesForGroup(int groupId)
        {
            return _context.ChatMessages.Include(x => x.Sender).Where(chatMessage => chatMessage.ChatGroupId == groupId).ToList();
        }

        public ICollection<ChatGroupConnection> GetChatgroupId(int id)
        {
            return _context.ChatGroupConnections.Include(x=> x.ChatGroup).Where(x => x.ApplicationUserId == id).ToList();
        }

        public Relative ConnectToCitizen(int relativeId, string connectionId)
        {
            var citizen = _context.Citizens.Include(x => x.ApplicationUser).SingleOrDefault(x => x.ConnectionId == connectionId);
            if (citizen == null) return null;
            var relative = _context.Relatives.Include(x => x.ApplicationUser).SingleOrDefault(x => x.RelativeId == relativeId);
            if (relative == null) return null;

            var connectedRelative = _context.Relatives.FirstOrDefault(x => x.CitizenId == citizen.CitizenId);
            if (connectedRelative != null)
            {
                var chatGroup = _context.ChatGroupConnections.Include(x => x.ChatGroup).FirstOrDefault(x => x.ApplicationUserId == connectedRelative.RelativeId && x.ChatGroup.GroupRole == 2);
                AddMemberToGroup(chatGroup.ChatGroupId, relativeId);
            }
            else {
                var newChatGroup = CreateChatGroup(citizen.CitizenId.ToString(), 2);
                AddMemberToGroup(newChatGroup.ChatGroupId, relativeId);
            }
            
            relative.CitizenId = citizen.CitizenId;
            
            _context.SaveChanges();
            return relative;
        }

        public bool CitizenConnectToCaregiverCenter(int citizenId, string connectionId)
        {
            var citizen = _context.Citizens.SingleOrDefault(x => x.CitizenId == citizenId);
            if (citizen == null) return false;
            var center = _context.CaregiverCenters.SingleOrDefault(x => x.CitizenConnectionId == connectionId);
            if (center == null) return false;
            citizen.CaregiverCenterId = center.CaregiverCenterId;

            var caregiverList = _context.Caregivers.Where(x => x.CaregiverCenterId == center.CaregiverCenterId).ToList();

            foreach (var caregiver in caregiverList)
            {
                var caretakerChatGroup = _context.ChatGroupConnections.Include(x=>x.ChatGroup).FirstOrDefault(x => x.ChatGroup.GroupName == citizenId.ToString() && x.ChatGroup.GroupRole == 3);
                AddMemberToGroup(caretakerChatGroup.ChatGroup.ChatGroupId, caregiver.CaregiverId);
            }
            

            _context.SaveChanges();
            return true;
        }

        public bool CaregiverConnectToCaregiverCenter(int caregiverId, string connectionId)
        {
            var caregiver = _context.Caregivers.SingleOrDefault(x => x.CaregiverId == caregiverId);
            if (caregiver == null) return false;
            var center = _context.CaregiverCenters.SingleOrDefault(x => x.CaregiverConnectionId == connectionId);
            if (center == null) return false;
            caregiver.CaregiverCenterId = center.CaregiverCenterId;

            var citizenList = _context.Citizens.Where(x=> x.CaregiverCenterId == center.CaregiverCenterId).ToList();
            foreach (var citizen in citizenList)
            {
                var caretakerChatGroup = _context.ChatGroupConnections.Include(x=> x.ChatGroup).FirstOrDefault(x => x.ChatGroup.GroupName == citizen.CitizenId.ToString() && x.ChatGroup.GroupRole == 3);
                AddMemberToGroup(caretakerChatGroup.ChatGroup.ChatGroupId, caregiverId);
            }

            _context.SaveChanges();
            return true;
        }

        public List<Relative> GetRelativesConnectedToId(int id)
        {
            return _context.Relatives.Include(x => x.ApplicationUser).Where(x => x.CitizenId == id).ToList();
        }

        public CaregiverCenter GetCaregiverCenterForCitizen(int id)
        {
            var citizen = _context.Citizens.SingleOrDefault(x => x.CitizenId == id);
            return _context.CaregiverCenters.SingleOrDefault(x => x.CaregiverCenterId == citizen.CaregiverCenterId);
        }

        public bool DeleteAppointment(int id)
        {
            var appointment = _context.Appointments.SingleOrDefault(x => x.AppointmentId == id);
            if (appointment == null) return false;
            _context.Appointments.Remove(appointment);
            _context.SaveChanges();
            return true;
        }

        public void CreateNote(Note note)
        {
            _context.Notes.Add(note);
            _context.SaveChanges();
        }

        public List<Note> GetNotes(int id)
        {
            return _context.Notes.Where(x => x.CitizenId == id).ToList();
        }

        public bool DeleteNote(int id)
        {
            var note = _context.Notes.SingleOrDefault(x => x.NoteId == id);
            if (note == null) return false;

            _context.Notes.Remove(note);
            _context.SaveChanges();
            return true;
        }

        //public ChatGroup CreateChatGroup(string name)
        //{
        //    var chatGroup = _context.ChatGroups.Add(new ChatGroup() {GroupName = name}).Entity;
        //    _context.SaveChanges();
        //    return chatGroup;
        //}

        public bool ChangeBoughtStatus(int id, bool bought)
        {
            var item = _context.ShoppingListItems.SingleOrDefault(x => x.ShoppingListItemId == id);
            if (item == null) return false;
            item.Bought = bought;
            _context.SaveChanges();
            return true;
        }

        public bool SetNewPrimaryRelative(int citizenId, int newPrimaryRelative)
        {
            var citizenChatGroup = _context.ChatGroupConnections.Include(x=>x.ChatGroup).FirstOrDefault(x => x.ApplicationUserId == citizenId && x.ChatGroup.GroupRole == 1);
            var caretakerChatGroup = _context.ChatGroups.FirstOrDefault(x => x.GroupName == citizenId.ToString() && x.GroupRole == 3);

            var newPrimary = _context.Relatives.SingleOrDefault(x => x.RelativeId == newPrimaryRelative);
            if (newPrimary == null) { return false; }

            var oldPrimary = _context.Relatives.SingleOrDefault(x => x.CitizenId == citizenId && x.PrimaryRelative);
            if (oldPrimary != null)
            {
                RemoveFromChatGroup(citizenChatGroup.ChatGroupId, oldPrimary.RelativeId); // removing old primary relative from citizen chat 
                RemoveFromChatGroup(caretakerChatGroup.ChatGroupId, oldPrimary.RelativeId); // removing old primary relative from caretaker chat
                oldPrimary.PrimaryRelative = false;
            }
            AddMemberToGroup(citizenChatGroup.ChatGroupId, newPrimary.RelativeId); // adding new primary relative to citizen chat
            AddMemberToGroup(caretakerChatGroup.ChatGroupId, newPrimary.RelativeId); // adding new primary relative to caretaker chat
            newPrimary.PrimaryRelative = true;

            _context.SaveChanges();
            return true;
        }

        public Appointment GetLatestAppointment(int id)
        {
            var appointment =
                _context.Appointments.Where(x => x.StartTime > DateTime.Now.ToUniversalTime() && x.CitizenId == id)
                    .OrderByDescending(x => x.StartTime)
                    .FirstOrDefault();
            return appointment;
        }
    }
}

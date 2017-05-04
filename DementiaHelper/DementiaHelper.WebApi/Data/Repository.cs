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
                    var chatGroup = CreateChatGroup(user.FirstName + " " + user.LastName);
                    user.ChatGroupId = chatGroup.ChatGroupId;
                    var citizen = new Citizen() {ApplicationUser = user, ConnectionId = connectionId};
                    _context.Add(citizen);
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
            _context.ChatMessages.Add(new ChatMessage() {ChatGroupId = groupId, Message = message, SenderId = sender});
            _context.SaveChanges();
        }

        public void AddMemberToGroup(int group, string email)
        {
            var targetUser = _context.ApplicationUsers.SingleOrDefault(user => user.Email == email);
            targetUser.ChatGroupId = group;
           _context.SaveChanges();
        }

        public ICollection<ChatMessage> GetChatMessagesForGroup(int groupId)
        {
            return _context.ChatMessages.Include(x => x.Sender).Where(chatMessage => chatMessage.ChatGroupId == groupId).ToList();
        }

        public Relative ConnectToCitizen(int relativeId, string connectionId)
        {
            var citizen = _context.Citizens.Include(x => x.ApplicationUser).SingleOrDefault(x => x.ConnectionId == connectionId);
            if (citizen == null) return null;
            var relative = _context.Relatives.Include(x => x.ApplicationUser).SingleOrDefault(x => x.RelativeId == relativeId);
            if (relative == null) return null;
            relative.CitizenId = citizen.CitizenId;
            relative.ApplicationUser.ChatGroupId = citizen.ApplicationUser.ChatGroupId;
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

        public ChatGroup CreateChatGroup(string name)
        {
            var chatGroup = _context.ChatGroups.Add(new ChatGroup() {GroupName = name}).Entity;
            _context.SaveChanges();
            return chatGroup;
        }

        public bool SetNewPrimaryRelative(int citizenId, int newPrimaryRelative)
        {
            var newPrimary = _context.Relatives.SingleOrDefault(x => x.RelativeId == newPrimaryRelative);
            if (newPrimary == null) { return false; }

            var oldPrimary = _context.Relatives.SingleOrDefault(x => x.CitizenId == citizenId && x.PrimaryRelative);
            if (oldPrimary != null){oldPrimary.PrimaryRelative = false;}

            newPrimary.PrimaryRelative = true;

            _context.SaveChanges();
            return true;
        }
    }
}

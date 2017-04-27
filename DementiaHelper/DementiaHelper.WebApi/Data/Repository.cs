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

namespace DementiaHelper.WebApi.Data
{
    public class Repository : IRepository
    {
        private readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public void CreateAccountInformation(string firstName, string lastName, string email, string description)
        {
            try
            {
                _context.ApplicationUsers.Add(new ApplicationUser()
                {
                    FirstName = firstName,
                    Lastname = lastName,
                    Email = email,
                    Description = description
                });
                _context.SaveChanges();

            }
            catch (Exception)
            {
                Console.WriteLine("Exception were thrown when creating AccountInformation in database");
                throw;
            }
        }

        public bool UpdateAccount(string firstName, string lastName, string email, string description)
        {
            try
            {
                //AccountInformation target = _context.AccountInformations.Find(email);
                var query = from p in _context.ApplicationUsers
                    where p.Email == email
                    select p;

                var target = query.SingleOrDefault();

                target.FirstName = firstName;
                target.Lastname = lastName;
                target.Email = email;
                target.Description = description;

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

        public Dictionary<string, object> GetAccount(string email)
        {
            try
            {
                var target = _context.ApplicationUsers.First(i=> i.Email == email);
                

                var values = new Dictionary<string, object>
            {
                {"FirstName", target.FirstName},
                {"LastName", target.Lastname},
                {"Email", target.Email},
                {"Description", target.Description}
            };
                
                return values;
            }
            catch (Exception)
            {
                Console.WriteLine("Exception were thrown when trying to get AccountInformation from database");
                return null;
            }
        }

        public bool CreateAccount(ApplicationUser user)
        {
            if (CheckIfUserExists(user.Email))
            {
                return false;
            }
            switch (user.RoleId)
            {
                case 1:
                    var citizen = new Citizen() {ApplicationUser = user/*, ApplicationUserForeignKey = 1*/}; //TODO: Remove ApplicationUserForeignKey from DB
                    _context.Add(citizen);
                    _context.SaveChanges();
                    return true;
                case 2:
                    var relative = new Relative() {ApplicationUser = user/*, ApplicationUserForeignKey = 1*/}; //TODO: Remove ApplicationUserForeignKey from DB
                    _context.Add(relative);
                    _context.SaveChanges();
                    return true;
                case 3:
                    var caregiver = new Caregiver() {ApplicationUser = user/*, ApplicationUserForeignKey = 1 */}; //TODO: Remove ApplicationUserForeignKey from DB
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
            return _context.Citizens.Include(x => x.ApplicationUser).Where(x => x.CaregiverId == id).ToList();
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

        public void SaveChatMessage(string message, int group, string sender)
        {
            _context.ChatMessages.Add(new ChatMessage() {ChatGroupId = Convert.ToInt32(group), Message = message, Sender = sender});
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
            return _context.ChatMessages.Where(chatMessage => chatMessage.ChatGroupId == groupId).ToList();
        }
    }
}

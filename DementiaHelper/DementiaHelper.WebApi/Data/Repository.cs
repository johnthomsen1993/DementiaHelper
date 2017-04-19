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
                _context.AccountInformations.Add(new AccountInformation()
                {
                    FirstName = firstName,
                    LastName = lastName,
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
                var query = from p in _context.AccountInformations
                    where p.Email == email
                    select p;

                var target = query.SingleOrDefault();

                target.FirstName = firstName;
                target.LastName = lastName;
                target.Email = email;
                target.Description = description;

                _context.AccountInformations.Update(target);
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

        public string CreateAccount(ApplicationUser user)
        {
            if (_context.ApplicationUsers.Any(o => o.Email == user.Email))
            {
                return "User already exists";
            }
            _context.ApplicationUsers.Add(user);
            _context.SaveChanges();
            return "User Created";

        }

        public ApplicationUser FetchApplicationUser(string email)
        {
            return _context.ApplicationUsers.FirstOrDefault(b => b.Email == email);
        }

        public bool CheckIfUserExists(string email)
        {
            if (_context.AccountInformations.Any(information => information.Email.Equals(email)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<ShoppingListDetail> GetShoppingList(int citizenId)
        {
            var queryable = _context.ShoppingListDetails.AsQueryable();

            Expression<Func<ShoppingListDetail, Product>> include_product = detail => detail.ProductForeignKey;
            Expression<Func<ShoppingListDetail, ShoppingList>> include_shoppinglist = detail => detail.ShoppingListForeignKey;

            return
                queryable.Where(
                    x => x.ShoppingListForeignKey.RelativeConnectionForeignKey.CitizenForeignKey.CitizenId == citizenId).Include(include_product).Include(include_shoppinglist)
                    .ToList();
        }

        public bool SaveItemInShoppingList(int shoppinglistId, string item, int quantity)
        {
            try
            {
                var query = from p in _context.Products
                            where p.ProductName == item.Trim()
                            select p;

                Product product = query.SingleOrDefault();

                if (product == null)
                {
                    product = _context.Products.Add(new Product() {ProductName = item}).Entity;
                    _context.SaveChanges();
                }

                var query2 = from p in _context.ShoppingLists
                            where p.ShoppingListId == shoppinglistId
                            select p;
                var shoppinglist = query2.SingleOrDefault();

                _context.ShoppingListDetails.Add(new ShoppingListDetail() {Bought = false, ProductForeignKey = product, Quantity = quantity, ShoppingListForeignKey = shoppinglist});

                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public bool RemoveShoppingListDetail(int id)
        {
            var item = _context.ShoppingListDetails.SingleOrDefault(x => x.ShoppingListDetailId == id);
            if (item == null) return false;
            _context.ShoppingListDetails.Remove(item);
            _context.SaveChanges();
            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using System.Linq;
using System.Security.Cryptography;
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
                AccountInformation target = _context.AccountInformations.Find(email);

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

        public AccountInformation GetAccount(string email)
        {
            try
            {
                AccountInformation target = _context.AccountInformations.Find(email);
                return target;
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
    }
}

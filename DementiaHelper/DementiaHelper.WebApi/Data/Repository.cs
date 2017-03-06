using System.Collections.Generic;
using System.Threading.Tasks;

using System.Linq;
using System.Security.Cryptography;
using DementiaHelper.WebApi.model;
using Microsoft.EntityFrameworkCore;

namespace DementiaHelper.WebApi.Data
{
    public class Repository : IRepository
    {
        private readonly ApplicationDbContext _context;
        public Repository(ApplicationDbContext context)
        {
            this._context = context;
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

    }
}

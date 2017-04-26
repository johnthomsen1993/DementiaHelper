using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace DementiaHelper.WebApi.model
{
    public class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        //public ICollection<ApplicationUser> ApplicationUsers { get; set; }
    }

}

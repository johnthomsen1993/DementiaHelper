using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;

namespace DementiaHelper.WebApi.model
{
    public class ApplicationUser
    {
        public int ApplicationUserId { get; set; }
        public string Email { get; set; }
        public string Salt { get; set; }
        public string Hash { get; set; }
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public string Description { get; set; }

        [ForeignKey("ChatGroup")]
        public int ChatGroupId { get; set; }
        public ChatGroup ChatGroup { get; set; }

        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}

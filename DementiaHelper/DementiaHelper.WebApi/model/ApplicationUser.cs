using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;

namespace DementiaHelper.WebApi.model
{
    public class ApplicationUser
    {
        [Key]
        public int ApplicationUserId { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Salt { get; set; }
        [Required]
        public string Hash { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Description { get; set; }
        [StringLength(15)]
        public string Phone { get; set; }

        [ForeignKey("ChatGroup")]
        public int? ChatGroupId { get; set; }
        public ChatGroup ChatGroup { get; set; }

        [Required]
        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace DementiaHelper.WebApi.model
{
    public class Citizen
    {
        [Key, ForeignKey("ApplicationUser")]
        public int CitizenId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        public string ConnectionId { get; set; }

        [ForeignKey("Caregiver")]
        public int? CaregiverId { get; set; }
        public Caregiver Caregiver { get; set; }
    }
}

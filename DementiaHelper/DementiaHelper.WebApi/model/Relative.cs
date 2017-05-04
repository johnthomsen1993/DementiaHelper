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
    public class Relative
    {
        [Key, ForeignKey("ApplicationUser")]
        public int RelativeId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        public bool PrimaryRelative { get; set; }

        [ForeignKey("Citizen")]
        public int? CitizenId { get; set; }
        public Citizen Citizen { get; set; }
    }
}

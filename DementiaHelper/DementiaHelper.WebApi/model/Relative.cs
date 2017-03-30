using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace DementiaHelper.WebApi.model
{
    public class Relative
    {
        [ForeignKey("ApplicationUser")]
        public int RelativeId { get; set; }
        public int ApplicationUserForeignKey { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }

}

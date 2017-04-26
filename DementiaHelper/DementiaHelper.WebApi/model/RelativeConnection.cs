using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace DementiaHelper.WebApi.model
{
    public class RelativeConnection
    {
        public int RelativeConnectionId { get; set; }

        [ForeignKey("Citizen")]
        public int CitizenId { get; set; }
        public Citizen Citizen { get; set; }

        [ForeignKey("Relative")]
        public int RelativeId { get; set; }
        public Relative Relative { get; set; }
    }
}

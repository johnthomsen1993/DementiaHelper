using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace DementiaHelper.WebApi.model
{
    public class CaregiverConnection
    {
        public int CaregiverConnectionId { get; set; }
        [ForeignKey("Citizen")]
        public Citizen CitizenForeignKey { get; set; }
        [ForeignKey("Caregiver")]
        public Caregiver CaregiverForeignKey { get; set; }
    }

}

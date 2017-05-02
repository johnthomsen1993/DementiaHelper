using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DementiaHelper.WebApi.model
{
    public class CaregiverCenter
    {
        [Key]
        public int CaregiverCenterId { get; set; }

        public string Name { get; set; }
        public int Phone { get; set; }

        [Required]
        public string CitizenConnectionId { get; set; }
        [Required]
        public string CaregiverConnectionId { get; set; }
    }
}

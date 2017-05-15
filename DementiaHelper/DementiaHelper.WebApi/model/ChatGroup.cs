using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DementiaHelper.WebApi.model
{
    public class ChatGroup
    {
        [Key]
        public int ChatGroupId { get; set; }
        [Required]
        public string GroupName { get; set; }
        public int? GroupRole { get; set; }
    }
}

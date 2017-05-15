using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DementiaHelper.WebApi.model
{
    public class ChatGroupConnection
    {
        [Key]
        public int ChatGroupConnectionId { get; set; }
        
        [Required]
        [ForeignKey("ChatGroup")]
        public int ChatGroupId { get; set; }
        public ChatGroup ChatGroup { get; set; }

        
        [Required]
        [ForeignKey("ApplicationUser")]
        public int ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}

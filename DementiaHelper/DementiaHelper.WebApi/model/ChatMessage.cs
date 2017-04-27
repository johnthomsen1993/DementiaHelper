﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace DementiaHelper.WebApi.model
{
    public class ChatMessage
    {
        [Key]
        public int ChatMessageId { get; set; }
        [Required]
        public string Sender { get; set; }
        [Required]
        public string Message { get; set; }

        [ForeignKey("ChatGroup")]
        public int ChatGroupId { get; set; }
        public ChatGroup ChatGroup { get; set; }
    }
}

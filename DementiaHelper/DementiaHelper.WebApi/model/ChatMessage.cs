using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace DementiaHelper.WebApi.model
{
    public class ChatMessage
    {
        public int ChatMessageId { get; set; }
        public string Sender { get; set; }
        public string Message { get; set; }

        [ForeignKey("ChatGroup")]
        public int GroupId { get; set; }
        public ChatGroup Chat { get; set; }
    }
}

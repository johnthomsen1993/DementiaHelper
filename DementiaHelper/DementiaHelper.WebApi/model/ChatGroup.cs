using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DementiaHelper.WebApi.model
{
    public class ChatGroup
    {
        public int ChatGroupId { get; set; }
        public string GroupName { get; set; }
    }
}

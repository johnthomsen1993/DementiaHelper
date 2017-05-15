using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DementiaHelper.Model
{
    class ChatGroup
    {
        public int ChatGroupId { get; set; }
        public int ApplicationUserId { get; set; }
        public int GroupRole { get; set; }
        public string GroupName { get; set; }
    }
}

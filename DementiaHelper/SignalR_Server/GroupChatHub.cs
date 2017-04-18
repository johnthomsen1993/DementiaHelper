using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace SignalR_Server
{
    [HubName("GroupChatHub")]
    public class GroupChatHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }

        [HubMethodName("groupChat")]
        public void BroadCastMessage(string name, string message, string groupName)
        {
            Clients.Group(groupName, null).receiveMessage(name, message);
        }
    }
}
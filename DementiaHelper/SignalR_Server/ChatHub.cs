﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace SignalR_Server
{
    
    public class ChatHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }

        [HubMethodName("singleChat")]
        public void BroadcastMessage(string name, string message)
        {
            var id = Context.ConnectionId;
            Clients.All.broadcastMessage(name, message);
        }

        [HubMethodName("groupChat")]
        public void BroadCastMessage(string name, string message, string groupName)
        {
            Clients.Group(groupName, null).receiveMessage(name, message);
        }
    }
}
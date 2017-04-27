using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace SignalR_Server
{
    
    public class ChatHub : Hub
    {
        private readonly WebService _databaService = new WebService();

        [HubMethodName("singleChat")]
        public void BroadcastMessage(string sender, string message)
        {
            Clients.All.broadcastMessage(sender, message);
        }

        [HubMethodName("groupChat")]
        public void BroadCastMessage(string sender, string message, string groupName)
        {
            _databaService.SaveMessage(message, groupName, sender);
            Clients.Group(groupName, null).GetMessage(sender, message);
        }

        public void JoinGroup(string groupName)
        {
            Groups.Add(Context.ConnectionId, groupName);
        }
    }
}
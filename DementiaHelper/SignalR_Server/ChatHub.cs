using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task BroadCastMessage(string sender, string message, string groupName)
        {
            JoinGroup(groupName);
            await _databaService.SaveMessage(message, groupName, sender);
            Clients.All.GetMessage(sender, message);
            Clients.Group(groupName).GetMessage(sender, message);
        }

        [HubMethodName("joinGroup")]
        public void JoinGroup(string groupName)
        {
            Groups.Add(Context.ConnectionId, groupName);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace SignalR_Server
{
    //Design and implemented from the following example from microsoft dokumentation
    //https://docs.microsoft.com/en-us/aspnet/signalr/overview/getting-started/tutorial-getting-started-with-signalr
    public class ChatHub : Hub
    {
        private readonly WebService _databaService = new WebService();

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        [HubMethodName("singleChat")]
        public void BroadcastMessage(string sender, string message)
        {
            Clients.All.broadcastMessage(sender, message);
        }

        /// <summary>
        /// Calls database service to save chatmassage and broadcast chatmessage to all in the group.
        /// </summary>
        /// <param name="sender">User id of the sender of the message</param>
        /// <param name="message">The message send</param>
        /// <param name="groupId">The group id of the group the message is send in</param>
        /// <param name="senderName">The name of the sender</param>
        /// <returns></returns>
        [HubMethodName("groupChat")]
        public async Task BroadCastMessage(int sender, string message, int groupId, string senderName)
        {
            JoinGroup(groupId.ToString());
            await _databaService.SaveMessage(message, groupId, sender);
            Clients.Group(groupId.ToString()).GetMessage(senderName, message);
        }

        /// <summary>
        /// Set the connection id form the user into a group
        /// </summary>
        /// <param name="groupName">Takes a string and set it as a name of the group to keep track of groups use the group id</param>
        [HubMethodName("joinGroup")]
        public void JoinGroup(string groupName)
        {
            Groups.Add(Context.ConnectionId, groupName);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DementiaHelper.Model;

namespace DementiaHelper.Services
{
    public interface IChatServices
    {
        Task Connect();
        Task Send(int sender, string message, int groupId, string senderName);
        Task JoinRoom(int groupId);
        event EventHandler<ChatMessage> OnMessageReceived;
    }

}

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
        Task Send(string sender, string message, int groupId);
        Task JoinRoom(int groupId);
        event EventHandler<ChatMessage> OnMessageReceived;
    }

}

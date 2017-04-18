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
        Task Send(ChatMessage message, string roomName);
        Task JoinRoom(string roomName);
        event EventHandler<ChatMessage> OnMessageReceived;
    }

}

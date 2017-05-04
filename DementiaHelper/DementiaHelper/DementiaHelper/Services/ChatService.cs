using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DementiaHelper.Model;
using DementiaHelper.Services;
using Microsoft.AspNet.SignalR.Client;
using Xamarin.Forms;


[assembly: Dependency(typeof(ChatServices))]

namespace DementiaHelper.Services
{
    public class ChatServices : IChatServices
    {
        private const string URL_test = "http://localhost:11306/";
        private const string URL = "http://dementiahelperchathub.azurewebsites.net/";
        private const string URL_start = "http://xamarin-chat.azurewebsites.net/";
        private readonly HubConnection _connection;
        private readonly IHubProxy _proxy;

        public event EventHandler<ChatMessage> OnMessageReceived;

        public ChatServices()
        {
            _connection = new HubConnection(URL);
            _proxy = _connection.CreateHubProxy("ChatHub");
        }

        #region IChatServices implementation

        public async Task Connect()
        {
            await _connection.Start();

            _proxy.On("GetMessage", (string name, string message) => OnMessageReceived(this, new ChatMessage
            {
                Name = name,
                Message = message
            }));
        }

        public async Task Send(int sender, string message, int groupId, string senderName)
        {
            await _connection.Start();
            _proxy.Invoke("groupChat", sender, message, groupId, senderName);
        }

        public async Task JoinRoom(int groupId)
        {
            _proxy.Invoke("joinGroup", groupId.ToString());
        }

        #endregion
    }
}


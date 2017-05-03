using DementiaHelper.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DementiaHelper.Services;
using FreshMvvm;
using Xamarin.Forms;
using System.Windows.Input;
using Newtonsoft.Json.Linq;
using PropertyChanged;

namespace DementiaHelper.PageModels
{
    [ImplementPropertyChanged]
    public class ChatPageModel : FreshMvvm.FreshBasePageModel 
    {
        
		private IChatServices _chatServices;
        private ApplicationUser user = (ApplicationUser)App.Current.Properties["ApplicationUser"];
        private const string URI_BASE = "http://dementiahelper.azurewebsites.net/api/chat/getMessagesForChatGroup/";
        private const string URI_BASE_TEST = "http://localhost:29342/api/chat/getMessagesForChatGroup/";
        private readonly int groupId;
        

        #region ViewModel Properties


        public ObservableCollection<Message> Messages { get; set; }

        public ChatMessage ChatMessage { get; set; }


        #endregion

        public ChatPageModel()
        {
            _chatServices = DependencyService.Get<IChatServices>();
            ChatMessage = new ChatMessage();
            Messages = new ObservableCollection<Message>();
            groupId = user.GroupId ?? 0;
            _chatServices.Connect();
            _chatServices.OnMessageReceived += _chatServices_OnMessageReceived;
            
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            _chatServices.JoinRoom(groupId);
            Device.BeginInvokeOnMainThread(async () =>
            {
                await GetChatMessageList(groupId);
            });
        }

        private async Task GetChatMessageList(int id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var encoded = JWTService.Encode(new Dictionary<string, object>() {{"GroupId", id}});
                    var result = await client.GetStringAsync(new Uri(URI_BASE + encoded));
                    var decoded = JWTService.Decode(result);
                    AddChatMessagesToList(decoded);
                }
                catch (Exception)
                {
                }
            }
        }

        private void AddChatMessagesToList(IDictionary<string, object> dict)
        {
            var list = dict["ChatMessageList"] as IList;
            
            foreach (var obj in list)
            {
                var jsonContainer = obj as JContainer;

                var chatMessageId = jsonContainer.SelectToken("ChatMessageId");
                var chatGroupId = jsonContainer.SelectToken("ChatGroupId");
                var senderId = jsonContainer.SelectToken("Sender").SelectToken("ApplicationUserId");
                var message = jsonContainer.SelectToken("Message");
                var firstName = jsonContainer.SelectToken("Sender").SelectToken("FirstName");
                var lastName = jsonContainer.SelectToken("Sender").SelectToken("FirstName");


                if (Convert.ToInt32(senderId) == user.ApplicationUserId)
                {
                    Messages.Add(new Message { Name = firstName + " " + lastName, MessageSent = message.ToString(), MessageRecievedIsVisible = false, MessageSentIsVisible = true });
                }
                else
                {
                    Messages.Add(new Message { Name = firstName + " " + lastName, MessageRecieved = message.ToString(), MessageRecievedIsVisible = true, MessageSentIsVisible = false });
                }
            }
        }

        void _chatServices_OnMessageReceived(object sender, ChatMessage e)
        {
            if (e.Name == user.FirstName + " " + user.LastName)
            {
                Messages.Add(new Message { Name = e.Name, MessageSent = e.Message, MessageRecievedIsVisible = false, MessageSentIsVisible = true });
                MessagingCenter.Send<ChatPageModel>(this, "New Messages");
            }
            else {
                Messages.Add(new Message { Name = e.Name, MessageRecieved = e.Message, MessageRecievedIsVisible=true,MessageSentIsVisible=false });
                MessagingCenter.Send<ChatPageModel>(this, "New Messages");
            }
        }
     

        #region Send Message Command

        Command sendMessageCommand;

        /// <summary>
        /// Command to Send Message
        /// </summary>
        public Command SendMessageCommand
        {
            get
            {
                return sendMessageCommand ??
                (sendMessageCommand = new Command(ExecuteSendMessageCommand));
            }
        }

        async void ExecuteSendMessageCommand()
        {
            var sender = user.ApplicationUserId;
           // IsBusy = true;
            await _chatServices.Send(sender, ChatMessage.Message, groupId, user.FirstName + " " + user.LastName);
         //   IsBusy = false;
        }

        #endregion

        #region Join Room Command

        Command joinRoomCommand;

        /// <summary>
        /// Command to Send Message
        /// </summary>
        public Command JoinRoomCommand
        {
            get
            {
                return joinRoomCommand ??
                    (joinRoomCommand = new Command(ExecuteJoinRoomCommand));
            }
        }

        async void ExecuteJoinRoomCommand()
        {
            //IsBusy = true;
            await _chatServices.JoinRoom(groupId);
            //IsBusy = false;
        }

        #endregion
    }
}
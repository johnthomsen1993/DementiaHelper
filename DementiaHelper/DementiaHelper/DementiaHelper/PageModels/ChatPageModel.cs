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


        public ObservableCollection<ChatMessage> Messages { get; set; }

        public ChatMessage ChatMessage { get; set; }


        #endregion

        public ChatPageModel()
        {
            _chatServices = DependencyService.Get<IChatServices>();
            ChatMessage = new ChatMessage();
            Messages = new ObservableCollection<ChatMessage>();
            groupId = user.GroupId ?? 0;

            Device.BeginInvokeOnMainThread(async () =>
            {
                await GetChatMessageList(groupId);
            });
            
            _chatServices.Connect();
            _chatServices.OnMessageReceived += _chatServices_OnMessageReceived;
            
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            _chatServices.JoinRoom(groupId);
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
                var sender = jsonContainer.SelectToken("Sender");
                var message = jsonContainer.SelectToken("Message");

                Messages.Add(new ChatMessage { Name = sender.ToString(), Message = message.ToString()});
            }
        }

        void _chatServices_OnMessageReceived(object sender, ChatMessage e)
        {
            Messages.Add(new ChatMessage { Name = e.Name, Message = e.Message});
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
            var sender = user.FirstName +  " hey " + user.LastName;
           // IsBusy = true;
            await _chatServices.Send(sender, ChatMessage.Message, groupId);
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
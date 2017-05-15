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
      //  private ApplicationUser user = (ApplicationUser)App.Current.Properties["ApplicationUser"];
        private const string URI_BASE = "http://dementiahelper.azurewebsites.net/api/chat/";
        private const string URI_BASE_TEST = "http://localhost:29342/api/chat/getMessagesForChatGroup/";
        private int groupId;
        private List<ChatGroup> chatGroupIds { get; set; }
        private int ChatRole { get; set; }
        

        #region ViewModel Properties


        public ObservableCollection<Message> Messages { get; set; }

        public ChatMessage ChatMessage { get; set; }


        #endregion

        public ChatPageModel()
        {
            _chatServices = DependencyService.Get<IChatServices>();
            ChatMessage = new ChatMessage();
            Messages = new ObservableCollection<Message>();
            chatGroupIds = new List<ChatGroup>();
        }

        public override void Init(object initData)
        {
            base.Init(initData);
            if (initData != null)
            {
                ChatRole = (int)initData;
            }
            
        }


        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            chatGroupIds.Clear();
            base.ViewIsAppearing(sender, e);
            Device.BeginInvokeOnMainThread(async () =>
            {
                await GetChatGroupId(((ApplicationUser)App.Current.Properties["ApplicationUser"]).ApplicationUserId);
                if (chatGroupIds.Count == 1)
                {
                    groupId = chatGroupIds.First().ChatGroupId;
                }
                else if (chatGroupIds.Count != 0)
                {
                    switch (ChatRole)
                    {
                        case 1:
                            var group1 = chatGroupIds.FirstOrDefault(x => x.GroupRole == 2);
                            groupId = group1.ChatGroupId;
                            break;
                        case 2:
                            var group2 = chatGroupIds.FirstOrDefault(x=> x.GroupRole == 2);
                            groupId = group2.ChatGroupId;
                            break;
                        case 3:
                            var group3 = chatGroupIds.FirstOrDefault(x => x.GroupRole == 2);
                            groupId = group3.ChatGroupId;
                            break;
                    }
                }
                await _chatServices.JoinRoom(groupId);
                await GetChatMessageList(groupId);
                _chatServices.OnMessageReceived += _chatServices_OnMessageReceived;

            });
        }

        protected override void ViewIsDisappearing(object sender, EventArgs e)
        {
            base.ViewIsDisappearing(sender, e);
            _chatServices.OnMessageReceived -= _chatServices_OnMessageReceived;
        }

        private async Task GetChatGroupId(int id)
        {
            using (var client = new HttpClient())
            {
                var encoded = JWTService.Encode(new Dictionary<string, object>() {{"ApplicationUserId", id}});
                var result = await client.GetStringAsync(new Uri(URI_BASE + "chatgroupid/" + encoded));
                var decoded = JWTService.Decode(result);
                MapChatGroupToList(decoded);
            }
        }

        private void MapChatGroupToList(IDictionary<string, object> dict)
        {
            var list = dict["ChatGroupIds"] as IList;

            foreach (var obj in list)
            {
                var jsonContainer = obj as JContainer;

                var chatGroupId = jsonContainer.SelectToken("ChatGroupId");
                var applicationUserId = jsonContainer.SelectToken("ApplicationUserId");
                var groupName = jsonContainer.SelectToken("ChatGroup").SelectToken("GroupName");
                var groupRole = jsonContainer.SelectToken("ChatGroup").SelectToken("GroupRole");

                chatGroupIds.Add(new ChatGroup() {ApplicationUserId = applicationUserId.ToObject<int>(), ChatGroupId = chatGroupId.ToObject<int>(), GroupName = groupName.ToString(), GroupRole = groupRole.ToObject<int>() });
            }
        }

        private async Task GetChatMessageList(int id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var encoded = JWTService.Encode(new Dictionary<string, object>() {{"GroupId", id}});
                    var result = await client.GetStringAsync(new Uri(URI_BASE + "getMessagesForChatGroup/" + encoded));
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
                var lastName = jsonContainer.SelectToken("Sender").SelectToken("LastName");


                if (senderId.ToObject<int>() == ((ApplicationUser)App.Current.Properties["ApplicationUser"]).ApplicationUserId)
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
            if (e.Name == ((ApplicationUser)App.Current.Properties["ApplicationUser"]).FirstName + " " + ((ApplicationUser)App.Current.Properties["ApplicationUser"]).LastName)
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
            var sender = ((ApplicationUser)App.Current.Properties["ApplicationUser"]).ApplicationUserId;
           // IsBusy = true;
            await _chatServices.Send(sender, ChatMessage.Message, groupId, ((ApplicationUser)App.Current.Properties["ApplicationUser"]).FirstName + " " + ((ApplicationUser)App.Current.Properties["ApplicationUser"]).LastName);
         //   IsBusy = false;
        }

        #endregion

        
    }
}
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

namespace DementiaHelper.PageModels
{
    public class ChatPageModel : ChatBasePageModel 
    {
        
		private IChatServices _chatServices;
        private int _roomId = 1;
        private int messageNumber = 0;
        public const string URI_BASE = "http://dementiahelper.azurewebsites.net/api/chat/getMessagesForChatGroup/";
        public const string URI_BASE_TEST = "http://localhost:29342/api/chat/getMessagesForChatGroup/";


        #region ViewModel Properties

        private ObservableCollection<ChatMassagePageModel> _messages;

        public ObservableCollection<ChatMassagePageModel> Messages
        {
            get { return _messages; }
            set
            {
                _messages = value;
                OnPropertyChanged("Messages");
            }
        }

        private ChatMassagePageModel _chatMessage;
        public ChatMassagePageModel ChatMessage
        {
            get { return _chatMessage; }
            set
            {
                _chatMessage = value;
                OnPropertyChanged("ChatMessage");
            }
        }


        #endregion

        public ChatPageModel()
        {
            _chatServices = DependencyService.Get<IChatServices>();
            _chatMessage = new ChatMassagePageModel();
            _messages = new ObservableCollection<ChatMassagePageModel>();

            Device.BeginInvokeOnMainThread(async () =>
            {
                await GetChatMessageList(_roomId);
                
            }); 

            _chatServices.Connect();
            _chatServices.JoinRoom(_roomId);
            _chatServices.OnMessageReceived += _chatServices_OnMessageReceived;
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
            
            foreach (var message in list)
            {
                //_messages.Add(obj);
            }
        }


        void _chatServices_OnMessageReceived(object sender, ChatMessage e)
        {
            _messages.Add(new ChatMassagePageModel {Name = e.Name, Message = e.Message, IsMine = _messages.Count%2 == 0});
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
            IsBusy = true;
            await _chatServices.Send(new ChatMessage { Name = _chatMessage.Name, Message = _chatMessage.Message }, _roomId.ToString());
            IsBusy = false;
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
            IsBusy = true;
            await _chatServices.JoinRoom(_roomId);
            IsBusy = false;
        }

        #endregion
    }
}

//public class ChatPageModel : FreshMvvm.FreshBasePageModel
//{
//    private string _newMessage { get; set; }
//    public string NewMessage
//    {
//        get { return _newMessage; }
//        set
//        {
//            _newMessage = value;
//            RaisePropertyChanged("NewMessage");
//        }
//    }
//    bool test123 { get; set; }
//    public ObservableCollection<Message> Messages { get; set; }
//    public ICommand SemdMessageommand { get; protected set; }

//    public ChatPageModel()
//    {
//        test123 = false;
//        Messages = new ObservableCollection<Message>();
//        SemdMessageommand = new Command(() => {
//            if (test123 == false)
//            {
//                SendMessage();
//                test123 = true;
//            }
//            else
//            {
//                RecievedMessage();
//                test123 = false;
//            }
//        });
//    }

//    private void SendMessage()
//    {
//        this.Messages.Add(new Message() { MessageSent = NewMessage, MessageRecieved = "", MessageRecievedIsVisible = false });
//    }
//    private void RecievedMessage()
//    {
//        this.Messages.Add(new Message() { MessageSent = "", MessageRecieved = NewMessage, MessageSentIsVisible = false });
//    }
//}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DementiaHelper.Model;
using DementiaHelper.Services;
using FreshMvvm;
using Xamarin.Forms;

namespace DementiaHelper.PageModels
{
    public class ChatPageModel : ChatBasePageModel 
    {
        
		private IChatServices _chatServices;
        private string _roomName = "PrivateRoom";


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
            _chatServices.Connect();
            _chatServices.OnMessageReceived += _chatServices_OnMessageReceived;
        }

        void _chatServices_OnMessageReceived(object sender, ChatMessage e)
        {
            _messages.Add(new ChatMassagePageModel { Name = e.Name, Message = e.Message, IsMine = _messages.Count % 2 == 0 });
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
            await _chatServices.Send(new ChatMessage { Name = _chatMessage.Name, Message = _chatMessage.Message }, _roomName);
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
            await _chatServices.JoinRoom(_roomName);
            IsBusy = false;
        }

        #endregion
    }
}


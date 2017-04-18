using DementiaHelper.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DementiaHelper.PageModels
{
    public class ChatPageModel : FreshMvvm.FreshBasePageModel
    {
        private string _newMessage { get; set; }
        public string NewMessage { get { return _newMessage; }
            set
            {
                _newMessage = value;
                RaisePropertyChanged("NewMessage");
            }
        }
        bool test123 { get; set; }
        public ObservableCollection<Message> Messages { get; set; }
        public ICommand SemdMessageommand { get; protected set; }

        public ChatPageModel(){
            test123 = false;
            Messages = new ObservableCollection<Message>();
            SemdMessageommand = new Command(() => {
                if (test123 == false) {
                    SendMessage();
                    test123 = true;
                    }
                else
                {
                    RecievedMessage();
                    test123 = false;
                }
            } );
        }

        private void SendMessage()
        {
            this.Messages.Add(new Message() {MessageSent= NewMessage, MessageRecieved= "", MessageRecievedIsVisible=false});
        }
        private void RecievedMessage()
        {
            this.Messages.Add(new Message() { MessageSent = "", MessageRecieved = NewMessage, MessageSentIsVisible = false });
        }
    }
}

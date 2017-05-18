using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DementiaHelper.Model.Controllers
{
    public interface IChatController
    {
        Task<List<ChatGroup>> GetChatGroupId(int id);
        Task<ObservableCollection<Message>> GetChatMessageList(int id);
        ObservableCollection<Message> AddChatMessagesToList(IDictionary<string, object> dict);
        List<ChatGroup> MapChatGroupToList(IDictionary<string, object> dict);
    }
}

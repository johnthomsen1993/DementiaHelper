using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DementiaHelper.Services;
using Newtonsoft.Json.Linq;

namespace DementiaHelper.Model.Controllers
{
    public class ChatController : IChatController
    {

        private const string URI_BASE = "http://dementiahelper.azurewebsites.net/api/chat/";
        public async Task<List<ChatGroup>> GetChatGroupId(int id)
        {
            using (var client = new HttpClient())
            {
                var encoded = JWTService.Encode(new Dictionary<string, object>() { { "ApplicationUserId", id } });
                var result = await client.GetStringAsync(new Uri(URI_BASE + "chatgroupid/" + encoded));
                var decoded = JWTService.Decode(result);
                return MapChatGroupToList(decoded);
            }
        }

        public List<ChatGroup> MapChatGroupToList(IDictionary<string, object> dict)
        {
            var list = dict["ChatGroupIds"] as IList;
            var tmpChatGroupList = new List<ChatGroup>();

            foreach (var obj in list)
            {
                var jsonContainer = obj as JContainer;
                var chatGroupId = jsonContainer.SelectToken("ChatGroupId");
                var applicationUserId = jsonContainer.SelectToken("ApplicationUserId");
                var groupName = jsonContainer.SelectToken("ChatGroup").SelectToken("GroupName");
                var groupRole = jsonContainer.SelectToken("ChatGroup").SelectToken("GroupRole");
                tmpChatGroupList.Add(new ChatGroup() { ApplicationUserId = applicationUserId.ToObject<int>(), ChatGroupId = chatGroupId.ToObject<int>(), GroupName = groupName.ToString(), GroupRole = groupRole.ToObject<int>() });
            }
            return tmpChatGroupList;
        }
        public ObservableCollection<Message> AddChatMessagesToList(IDictionary<string, object> dict)
        {
            var list = dict["ChatMessageList"] as IList;

            var tmpMessageList = new ObservableCollection<Message>();
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
                    tmpMessageList.Add(new Message { Name = firstName + " " + lastName, MessageSent = message.ToString(), MessageRecievedIsVisible = false, MessageSentIsVisible = true });
                }
                else
                {
                    tmpMessageList.Add(new Message { Name = firstName + " " + lastName, MessageRecieved = message.ToString(), MessageRecievedIsVisible = true, MessageSentIsVisible = false });
                }
            }
            return tmpMessageList;
        }
        public async Task<ObservableCollection<Message>> GetChatMessageList(int id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var encoded = JWTService.Encode(new Dictionary<string, object>() { { "GroupId", id } });
                    var result = await client.GetStringAsync(new Uri(URI_BASE + "getMessagesForChatGroup/" + encoded));
                    var decoded = JWTService.Decode(result);
                    return AddChatMessagesToList(decoded);
                }
                catch (Exception)
                {
                    return new ObservableCollection<Message>();
                }
            }
        }
    }
}

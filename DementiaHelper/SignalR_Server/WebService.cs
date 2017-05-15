using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace SignalR_Server
{
    public class WebService
    {
        public const string URI_BASE = "http://dementiahelper.azurewebsites.net/api/chat/saveChatMessage/";

        public async Task SaveMessage(string message, int groupId, int sender)
        {
            var payload = new Dictionary<string, object>
            {
                {"Message", message},
                {"GroupId", groupId},
                {"Sender", sender}
            };

            var encoded = JWTService.Encode(payload);

            using (HttpClient h = new HttpClient())
            {
                var values = new Dictionary<string, string> {{"token", encoded}};
                var content = new FormUrlEncodedContent(values);
                await h.PutAsync(new Uri(URI_BASE), content);
            }
        }
    }
}
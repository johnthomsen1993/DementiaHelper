﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace SignalR_Server
{
    public class WebService
    {
        public const string URI_BASE = "http://dementiahelper.azurewebsites.net/api/chat/saveChatMessage/";

        public void SaveMessage(string message, string groupId, string sender)
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
                var values = new Dictionary<string, string> { { "content", encoded } };
                var content = new FormUrlEncodedContent(values);
                var result = h.PutAsync(new Uri(URI_BASE), content).Result;
            }
        }
    }
}
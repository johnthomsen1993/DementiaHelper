using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DementiaHelper.WebApi.Data;
using DementiaHelper.WebApi.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace DementiaHelper.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ChatController : Controller
    {
        private readonly IRepository _iRepository;
        public ChatController(IRepository iRepository)
        {
            _iRepository = iRepository;
        }

        [HttpPut("saveChatMessage")]
        [AllowAnonymous]
        public void SaveChatMessage(string token)
        {
            var decoded = JWTService.Decode(token);
            if (Convert.ToInt32(decoded["GroupId"]) != 0)
            {
                _iRepository.SaveChatMessage(decoded["Message"]?.ToString(), Convert.ToInt32(decoded["GroupId"]), Convert.ToInt32(decoded["Sender"]));
            }
            
        }

        [HttpGet("getMessagesForChatGroup/{token}")]
        [AllowAnonymous]
        public string Get(string token)
        {
            var decoded = JWTService.Decode(token);
            var chatMessageList = _iRepository.GetChatMessagesForGroup(Convert.ToInt32(decoded["GroupId"]));
            var payload = new Dictionary<string, object>()
            {
                {"ChatMessageList", chatMessageList}
            };
            return JWTService.Encode(payload);
        }

        [HttpPut("addMemberToChatGroup")]
        [AllowAnonymous]
        public void AddMemberToGroup(string token)
        {
            var decoded = JWTService.Decode(token);
            _iRepository.AddMemberToGroup(Convert.ToInt32(decoded["GroupId"]), decoded["Email"]?.ToString());
        }
    }
}

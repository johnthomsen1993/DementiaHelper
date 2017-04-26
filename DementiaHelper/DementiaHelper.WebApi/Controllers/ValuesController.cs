using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DementiaHelper.WebApi.Data;
using DementiaHelper.WebApi.model;
using DementiaHelper.WebApi.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DementiaHelper.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private IRepository _iRepository;
        public ValuesController(IRepository iRepository)
        {
            _iRepository = iRepository;
        }

        // POST api/values
        [HttpGet("getspecific/{token}")]
        [AllowAnonymous]
        public string GetUserAccount(string token)
        {
            var decoded = JWTService.Decode(token);
            var userAccount = _iRepository.GetAccount(decoded["Email"]?.ToString());
            var encoded = JWTService.Encode(userAccount);
            return encoded;
        }

        [HttpPost("save")]
        [AllowAnonymous]
        public string Post(string token)
        {
            var decoded = JWTService.Decode(token);
            if (_iRepository.CheckIfUserExists(decoded["Email"]?.ToString()))
            {
                bool succes = _iRepository.UpdateAccount(decoded["FirstName"]?.ToString(),
                    decoded["LastName"]?.ToString(), decoded["Email"]?.ToString(), decoded["Description"]?.ToString());
                if (succes)
                {
                    return "The data is saved";
                }
                return "Mistakes were made";
            }
            else
            {
                _iRepository.CreateAccountInformation(decoded["FirstName"]?.ToString(),
                    decoded["LastName"]?.ToString(), decoded["Email"]?.ToString(), decoded["Description"]?.ToString());
                return "New user saved in the database";
            }
        }

        // DELETE api/values/shoppinglist/{token}
        [HttpDelete("shoppinglist/{token}")]
        [AllowAnonymous]
        public string DeleteItemFromList(string token)
        {
            var decoded = JWTService.Decode(token);
            var id = Convert.ToInt32(decoded.SingleOrDefault(x => x.Key.Equals("shoppingListDetailId")).Value);
            var removed = _iRepository.RemoveShoppingListDetail(id);
            string encoded;
            if (removed)
            {
                var shoppingList = _iRepository.GetShoppingList(Convert.ToInt32(decoded.SingleOrDefault(x => x.Key.Equals("citizenId")).Value));
                encoded = JWTService.Encode(new Dictionary<string, object>() {{ "ShoppingList", shoppingList}});
                return encoded;
            }
            else
            {
                encoded = JWTService.Encode(new Dictionary<string, object>() {{"ErrorOnRemove", removed}});
                return encoded;
            }
        }
        
        // GET api/values/shoppinglist/{token}
        [HttpGet("shoppinglist/{token}")]
        [AllowAnonymous]
        public string GetShoppingList(string token)
        {
            var decoded = JWTService.Decode(token);
            var id = Convert.ToInt32(decoded["citizenId"]);
            var shoppingList = _iRepository.GetShoppingList(id);
            var payload = new Dictionary<string, object>()
            {
                {"ShoppingList", shoppingList}
            };
            var encoded = JWTService.Encode(payload);
            return encoded;
        }

        // PUT api/values/5
        [HttpPut("shoppinglist")]
        [AllowAnonymous]
        public bool Put(string content)
        {
            var decoded = JWTService.Decode(content);
            var sucess = _iRepository.SaveItemInShoppingList(Convert.ToInt32(decoded["ShoppingListId"]), decoded["Item"]?.ToString(), Convert.ToInt32(decoded["Quantity"]));
            return sucess;
        }

        [HttpGet("calendar/{token}")]
        [AllowAnonymous]
        public string GetAppointments(string token)
        {
            var decoded = JWTService.Decode(token);
            var appointments = _iRepository.GetAppointments(Convert.ToInt32(decoded.SingleOrDefault(x => x.Key.Equals("CitizenId")).Value));
            return JWTService.Encode(new Dictionary<string, object>() {{"Appointments", appointments}});
        }

        [HttpPut("calendar")]
        [AllowAnonymous]
        public string CreateAppointment(string content)
        {
            var decoded = JWTService.Decode(content);
            try
            {
                var appointment = new Appointment()
                {
                    CitizenId = Convert.ToInt32(decoded.SingleOrDefault(x => x.Key.Equals("CitizenId")).Value),
                    Subject = decoded.SingleOrDefault(x => x.Key.Equals("Subject")).Value.ToString(),
                    StartTime = DateTime.Parse(decoded.SingleOrDefault(x => x.Key.Equals("StartTime")).Value.ToString()),
                    EndTime = DateTime.Parse(decoded.SingleOrDefault(x => x.Key.Equals("EndTime")).Value.ToString()),
                    Color = decoded.SingleOrDefault(x => x.Key.Equals("Color")).Value.ToString()
                };
                _iRepository.CreateAppointment(appointment);
                return JWTService.Encode(new Dictionary<string, object>() { { "Success", true } });
            }
            catch (Exception)
            {
                return JWTService.Encode(new Dictionary<string, object>() { { "Success", false } });
            }
        }
    }
}

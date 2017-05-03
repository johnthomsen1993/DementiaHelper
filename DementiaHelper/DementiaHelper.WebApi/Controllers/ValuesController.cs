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

        // DELETE api/values/shoppinglist/{token}
        [HttpDelete("shoppinglist/{token}")]
        [AllowAnonymous]
        public string DeleteItemFromList(string token)
        {
            var decoded = JWTService.Decode(token);
            var id = Convert.ToInt32(decoded.SingleOrDefault(x => x.Key.Equals("shoppingListItemId")).Value);
            var removed = _iRepository.RemoveShoppingListItem(id);
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
            if (shoppingList == null) return JWTService.Encode(new Dictionary<string, object>() {{"List", false}});
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
        public void Put(string content)
        {
            var decoded = JWTService.Decode(content);
            var sucess = _iRepository.SaveItemInShoppingList(Convert.ToInt32(decoded["CitizenId"]), decoded["Item"]?.ToString(), Convert.ToInt32(decoded["Quantity"]));
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

        [HttpDelete("calendar")]
        [AllowAnonymous]
        public string DeleteAppointment(string token)
        {
            var decoded = JWTService.Decode(token);
            var removed = _iRepository.DeleteAppointment(Convert.ToInt32(decoded.SingleOrDefault(x => x.Key.Equals("AppointmentId")).Value));
            return JWTService.Encode(new Dictionary<string, object>() { { "Removed", removed } });
        }

        [HttpPut("note")]
        [AllowAnonymous]
        public string CreateNote(string content)
        {
            var decoded = JWTService.Decode(content);
            try
            {
                var note = new Note()
                {
                    CitizenId = Convert.ToInt32(decoded.SingleOrDefault(x => x.Key.Equals("CitizenId")).Value),
                    Subject = decoded.SingleOrDefault(x => x.Key.Equals("Subject")).Value.ToString(),
                    CreatedTime = DateTime.Parse(decoded.SingleOrDefault(x => x.Key.Equals("Subject")).Value.ToString())
                };
                _iRepository.CreateNote(note);
                return JWTService.Encode(new Dictionary<string, object>() {{"NoteCreated", true}});
            }
            catch (Exception)
            {
                return JWTService.Encode(new Dictionary<string, object>() {{"NoteCreated", false}});
            }
            
        }

        [HttpGet("note/{token}")]
        [AllowAnonymous]
        public string GetNotes(string token)
        {
            var decoded = JWTService.Decode(token);
            var notes =
                _iRepository.GetNotes(Convert.ToInt32(decoded.SingleOrDefault(x => x.Key.Equals("CitizenId")).Value));
            return JWTService.Encode(new Dictionary<string, object>() { {"Notes", notes} });
        }

        [HttpDelete("note")]
        [AllowAnonymous]
        public string DeleteNote(string token)
        {
            var decoded = JWTService.Decode(token);
            var deleted =
                _iRepository.DeleteNote(Convert.ToInt32(decoded.SingleOrDefault(x => x.Key.Equals("NoteId")).Value));
            return JWTService.Encode(new Dictionary<string, object>() { {"Deleted", deleted} });
        }
    }
}

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

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(string token)
        //{
        //}

        // POST api/values
        [HttpPost("getspecific")]
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


        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [Route("api/[controller]/ShoppingList/{token}")]
        [HttpGet]
        public string GetShoppingList(string token)
        {
            var decoded = JWTService.Decode(token);
            var shoppingList = _iRepository.GetShoppingList(decoded["citizenId"]?.ToString());
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
    }
}

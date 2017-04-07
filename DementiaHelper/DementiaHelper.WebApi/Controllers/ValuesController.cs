using System;
using System.Collections.Generic;
using System.Linq;
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
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost("getspecific")]
        [AllowAnonymous]
        public string Post(string email)
        {
            return "Yay";
            //return _iRepository.GetAccount(email);
        }

        [HttpPost("save")]
        [AllowAnonymous]
        public string Post(string firstName, string lastName, string email, string description)
        {
            if (_iRepository.CheckIfUserExists(email))
            {
                bool succes = _iRepository.UpdateAccount(firstName, lastName, email, description);
                if (succes)
                {
                    return "The data is saved";
                }
                return "Mistakes were made";
            }
            else
            {
                _iRepository.CreateAccountInformation(firstName, lastName, email, description);
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
        
        // GET api/values/shoppinglist/GAGfwgewegopmXEOM
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
    }
}

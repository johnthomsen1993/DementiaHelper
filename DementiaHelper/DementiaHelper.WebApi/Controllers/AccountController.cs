using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using DementiaHelper.WebApi.Data;
using DementiaHelper.WebApi.model;
using DementiaHelper.WebApi.Options;
using DementiaHelper.WebApi.Service;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace DementiaHelper.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private static readonly RandomNumberGenerator Random = RandomNumberGenerator.Create();
        private readonly IRepository _repository;
        // GET: /<controller>/

        public AccountController(IRepository repository)
        {
            _repository = repository;
        }

        //email, password, role
        [HttpPut("createAccount")]
        [AllowAnonymous]
        public string CreateAccount(string token)
        {
            var decoded = JWTService.Decode(token);
            var user = new ApplicationUser()
            {
                Email = decoded.SingleOrDefault(x => x.Key.Equals("email")).ToString(),
                Role = new Role() {RoleId = Convert.ToInt32(decoded.SingleOrDefault(x => x.Key.Equals("RoleId")))},
                Salt = GenerateSalt()
            };
            user.Hash = GenerateHash(decoded.SingleOrDefault(x => x.Key.Equals("password")).ToString(),user.Salt);
            return JWTService.Encode(new Dictionary<string, object>() { {"UserCreated", _repository.CreateAccount(user) } });
        }

        private string GenerateHash(string password, string salt)
        {
            var hash=Convert.ToBase64String(KeyDerivation.Pbkdf2(
             password: password,
             salt: Convert.FromBase64String(salt),
             prf: KeyDerivationPrf.HMACSHA1,
             iterationCount: 10000,
             numBytesRequested: 256 / 8));
            return hash;
        }

        private bool ComparePasswords(string password, string salt, string hashedPassword)
        {
            return GenerateHash(password, salt) == hashedPassword;
        }

        //email, password
        [HttpGet("login")]
        [AllowAnonymous]
        public string Login(string token)
        {
            var decoded = JWTService.Decode(token);
            var user =_repository.FetchApplicationUser(decoded["email"].ToString());
            if (user == null) return JWTService.Encode(new Dictionary<string, object>() {{"UserExists", false}});
            if (!ComparePasswords(decoded.SingleOrDefault(x => x.Key.Equals("password")).ToString(), user.Salt, user.Hash))
                return JWTService.Encode(new Dictionary<string, object>() {{"password", false}});
            var payload = new Dictionary<string, object> {{"user", user}};
            switch (user.Role.RoleId)
            {
                case 0:
                    break;
                case 1:
                    var relativeConnection = _repository.GetRelativeConnection(user.ApplicationUserId);
                    payload.Add("citizenId", relativeConnection.CitizenForeignKey.CitizenId);
                    break;
                case 2:
                    var caregiverConnection = _repository.GetCaregiverConnections(user.ApplicationUserId);
                    var list = new List<int>();
                    caregiverConnection.ForEach(x => list.Add(x.CitizenForeignKey.CitizenId));
                    payload.Add("citizenIds", list);
                    break;
                default:
                    return JWTService.Encode(new Dictionary<string, object>() {{"ErrorRole", false}});
            }
            return JWTService.Encode(payload);
        }
        

        private string GenerateSalt()
        {
            var key = new byte[256 / 8];
            Random.GetBytes(key);
            return Convert.ToBase64String(key);
        }
    }
}

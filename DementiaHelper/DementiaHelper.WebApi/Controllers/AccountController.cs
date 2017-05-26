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
using System.Security.Cryptography.X509Certificates;
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
                Email = decoded["email"].ToString(),
                RoleId = Convert.ToInt32(decoded["role"]),
                FirstName = decoded["firstName"].ToString(),
                LastName = decoded["lastName"].ToString(),
                Salt = GenerateSalt()
            };
            user.Hash = GenerateHash(decoded.SingleOrDefault(x => x.Key.Equals("password")).Value.ToString(),user.Salt);
            var success = user.RoleId == 1 ? _repository.CreateAccount(user, GenerateConnectionId()) : _repository.CreateAccount(user);
            return JWTService.Encode(new Dictionary<string, object>() { {"UserCreated", success } });
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
        public string Login([FromHeader]string token)
        {
            var decoded = JWTService.Decode(token);
            var user =_repository.FetchApplicationUser(decoded["email"].ToString());
            if (user == null) return JWTService.Encode(new Dictionary<string, object>() {{"UserExists", false}});

            if (!ComparePasswords(decoded.SingleOrDefault(x => x.Key.Equals("password")).Value.ToString(), user.Salt, user.Hash))
                return JWTService.Encode(new Dictionary<string, object>() {{"Password", false}});

            var payload = new Dictionary<string, object> {{"User", user}};
            switch (user.Role.RoleId)
            {
                case 1:
                    var citizen = _repository.GetCitizen(user.ApplicationUserId);
                    if (citizen == null) break;
                    payload.Add("ConnectionId", citizen.ConnectionId);
                    break;
                case 2:
                    var relative = _repository.GetRelative(user.ApplicationUserId);
                    if (relative == null) break;
                    payload.Add("CitizenId", relative.CitizenId);
                    payload.Add("PrimaryRelative", relative.PrimaryRelative);
                    break;
                case 3:
                    var caregiver = _repository.GetCaregiver(user.ApplicationUserId);
                    if (caregiver?.CaregiverCenterId != null)
                    {
                        var citizens = _repository.GetCitizenList(caregiver.CaregiverCenterId.Value);
                        var list = new List<Citizen>();
                        citizens.ForEach(x => list.Add(new Citizen() { CitizenId = x.CitizenId, ApplicationUser = new ApplicationUser() { FirstName = x.ApplicationUser.FirstName, LastName = x.ApplicationUser.LastName } }));
                        payload.Add("CitizenIds", list);
                    }
                    break;
                default:
                    return JWTService.Encode(new Dictionary<string, object>() {{"ErrorRole", false}});
            }
            return JWTService.Encode(payload);
        }

        [HttpPut("update")]
        [AllowAnonymous]
        public string UpdateAccountInforamtion(string token)
        {
            var decoded = JWTService.Decode(token);
            var user = new ApplicationUser()
            {
                Email = decoded["email"]?.ToString(),
                FirstName = decoded["firstName"]?.ToString(),
                LastName = decoded["lastName"]?.ToString(),
                Description = decoded["description"]?.ToString(),
                Phone = decoded["phone"]?.ToString()
            };
            var success = _repository.UpdateAccount(user, decoded["oldEmail"].ToString());
            return JWTService.Encode(new Dictionary<string, object>() { { "UserUpdated", success } });
        }

        [HttpGet("getuser/{token}")]
        [AllowAnonymous]
        public string GetUserInformation(string token)
        {
            var decoded = JWTService.Decode(token);
            var user = _repository.GetApplicationUser(decoded["email"]?.ToString());
            var payload = new Dictionary<string, object>
            {
                {"firstName", user.FirstName},
                {"lastName", user.LastName},
                {"email", user.Email},
                {"description", user.Description},
                {"roleId", user.RoleId },
                {"phone", user.Phone}
            };
            var encoded = JWTService.Encode(payload);
            return encoded;
        }

        [HttpGet("contactlist/{token}")]
        [AllowAnonymous]
        public string GetListOfConnectedUsers(string token)
        {
            var decoded = JWTService.Decode(token);
            var users = _repository.GetRelativesConnectedToId(Convert.ToInt32(decoded["CitizenId"]));
            var caregiverCenter = _repository.GetCaregiverCenterForCitizen(Convert.ToInt32(decoded["CitizenId"]));

            var payload = new Dictionary<string, object>
            {
                {"contactList", users},
                {"caregiverCenter", caregiverCenter}
            };

            var encoded = JWTService.Encode(payload);
            return encoded;
        }

        private string GenerateSalt()
        {
            var key = new byte[256 / 8];
            Random.GetBytes(key);
            return Convert.ToBase64String(key);
        }

        private string GenerateConnectionId()
        {
            var g = Guid.NewGuid();
            var guidString = Convert.ToBase64String(g.ToByteArray());
            guidString = guidString.Replace("=", "");
            guidString = guidString.Replace("+", "");
            guidString = guidString.Replace("/" +
                                            "" +
                                            "" +
                                            "" +
                                            "" +
                                            "" +
                                            "" +
                                            "", "");
            return guidString.Substring(0, 7);
        }

        [HttpPut("connecttocitizen")]
        [AllowAnonymous]
        public string ConnectToCitizen(string token)
        {
            var decoded = JWTService.Decode(token);
            var relative = _repository.ConnectToCitizen(Convert.ToInt32(
                decoded.SingleOrDefault(x => x.Key.Equals("RelativeId")).Value),
                decoded.SingleOrDefault(x => x.Key.Equals("ConnectionId")).Value.ToString());
            if (relative == null) return JWTService.Encode(new Dictionary<string, object>() {{"Connected", false}});

            var payload = new Dictionary<string, object>()
            {
                {"User", relative.ApplicationUser},
                {"CitizenId", relative.CitizenId},
                {"PrimaryRelative", relative.PrimaryRelative }
            };
            return JWTService.Encode(payload);
        }

        [HttpPut("connectcitizentocenter")]
        [AllowAnonymous]
        public string ConnectToCaregiverCenter(string token)
        {
            var decoded = JWTService.Decode(token);
            return JWTService.Encode(new Dictionary<string, object>()
            {
                {
                    "Connected", _repository.CitizenConnectToCaregiverCenter(Convert.ToInt32(
                        decoded.SingleOrDefault(x => x.Key.Equals("CitizenId")).Value), decoded.SingleOrDefault(x => x.Key.Equals("ConnectionId")).Value.ToString())
                }
            });
        }

        [HttpPut("primaryrelative")]
        [AllowAnonymous]
        public string SetPrimaryRelative(string token)
        {
            var decoded = JWTService.Decode(token);
            var success = _repository.SetNewPrimaryRelative(Convert.ToInt32(decoded["CitizenId"]), Convert.ToInt32(decoded["NewPrimaryRelative"]));
            return JWTService.Encode(new Dictionary<string, object>() { { "PrimaryRelativeChanged", success } });
        }
    }
}

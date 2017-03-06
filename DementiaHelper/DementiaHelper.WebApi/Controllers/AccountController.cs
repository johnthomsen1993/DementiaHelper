using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using DementiaHelper.WebApi.Data;
using DementiaHelper.WebApi.model;
using DementiaHelper.WebApi.Options;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace DementiaHelper.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private static readonly RandomNumberGenerator Random = RandomNumberGenerator.Create();
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly ILogger _logger;
        private readonly JsonSerializerSettings _serializerSettings;
        private readonly IRepository _repository;
        // GET: /<controller>/

        public AccountController(IOptions<JwtIssuerOptions> jwtOptions, ILoggerFactory loggerFactory, IRepository repository)
        {
            _repository = repository;
            _jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_jwtOptions);
            _logger = loggerFactory.CreateLogger<AccountController>();
            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }

        [HttpPost("createAccount")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAccount(string email, string password)
        {
            var user = new ApplicationUser() {Email = email, Salt = GenerateSalt() };
            user.Hash = GenerateHash(password,user.Salt);
            if (_repository.CreateAccount(user) == "User already exists") {return new OkObjectResult("User already exists"); }
            var identity = await GetClaimsIdentity(email, password);
            if (identity == null)
            {
                _logger.LogInformation($"Invalid username ({email}) or password ({password})");
                return BadRequest("Invalid credentials");
            }

            var claims = new[]
            {
               new Claim(JwtRegisteredClaimNames.Sub, email),
               new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
               new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
               identity.FindFirst("DementiaHelper")
            };
            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: _jwtOptions.NotBefore,
                expires: _jwtOptions.Expiration,
                signingCredentials: _jwtOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            
            // Serialize and return the response
            var response = new
            {
                access_token = encodedJwt,
                expires_in = (int)_jwtOptions.ValidFor.TotalSeconds
            };

            var json = JsonConvert.SerializeObject(response, _serializerSettings);
            return new OkObjectResult(json);
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
            if (GenerateHash(password, salt) == hashedPassword)
            {
                return true;
            }
            return false;
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user =_repository.FetchApplicationUser(email);
            if (user == null)
            {
                return new OkObjectResult("User not found");
            }
            var identity = await GetClaimsIdentity(email,password);
            if (identity == null)
            {
                _logger.LogInformation($"Invalid username ({email}) or password ({password})");
                return BadRequest("Invalid credentials");
            }

            var claims = new[]
            {
               new Claim(JwtRegisteredClaimNames.Sub, email),
               new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
               new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
               identity.FindFirst("DementiaHelper")
            };
            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: _jwtOptions.NotBefore,
                expires: _jwtOptions.Expiration,
                signingCredentials: _jwtOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            // Serialize and return the response
            var response = new
            {
                access_token = encodedJwt,
                expires_in = (int)_jwtOptions.ValidFor.TotalSeconds
            };

            var json = JsonConvert.SerializeObject(response, _serializerSettings);
            return new OkObjectResult(json);
        }
        /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        /// <summary>
        /// IMAGINE BIG RED WARNING SIGNS HERE!
        /// You'd want to retrieve claims through your claims provider
        /// in whatever way suits you, the below is purely for demo purposes!
        /// </summary>
        private static Task<ClaimsIdentity> GetClaimsIdentity(string email, string password)
        {
            if (email == "john" &&
               password == "password")
            {
                return Task.FromResult(new ClaimsIdentity(new GenericIdentity(email, "Token"), new[]{new Claim("DementiaHelper", "Dementia") }));
            }

            // Credentials are invalid, or account doesn't exist
            return Task.FromResult<ClaimsIdentity>(null);
        }


        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
            }
        }
        private string GenerateSalt()
        {
            var key = new byte[256 / 8];
            Random.GetBytes(key);
            return Convert.ToBase64String(key);
        }
    }
}

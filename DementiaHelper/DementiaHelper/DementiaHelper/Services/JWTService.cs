using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JWT.exceptions;

namespace DementiaHelper.Services
{
    // adapted from example at https://github.com/senzacionale/jwt-portable-dotnet
    public static class JWTService
    {
        private const string SecretKey = "";
        public static string Encode(IDictionary<string, object> payload)
        {
            return JWT.JsonWebToken.Encode(payload, SecretKey, JWT.JwtHashAlgorithm.HS256);
        }

        public static IDictionary<string, object> Decode(string token)
        {
            IDictionary<string, object> payload = null;
            try
            {
                payload = JWT.JsonWebToken.DecodeToObject(token, SecretKey) as IDictionary<string, object>;
            }
            catch (JWT.exceptions.SignatureVerificationException)
            {
                // TODO: make log trace
                //Console.WriteLine(e.StackTrace);
            }
            return payload;
        }
    }
}

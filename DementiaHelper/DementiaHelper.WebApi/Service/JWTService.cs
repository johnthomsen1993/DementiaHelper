using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DementiaHelper.WebApi.Service
{
    // Adapted from example at https://github.com/SiroccoHub/JwtCore/
    public static class JWTService
    {
        private const string SecretKey = "1234";
        public static string Encode(IDictionary<string, object> payload)
        {
            return JwtCore.JsonWebToken.Encode(payload, SecretKey, JwtCore.JwtHashAlgorithm.HS256);
        }

        public static IDictionary<string, object> Decode(string token)
        {
            IDictionary<string, object> payload = null;
            try
            {
                payload =  JwtCore.JsonWebToken.DecodeToObject(token, SecretKey) as IDictionary<string, object>;
            }
            catch (JwtCore.SignatureVerificationException)
            {
                // TODO: make log trace
                //Console.WriteLine(e.StackTrace);
            }
            return payload;
        }
    }
}

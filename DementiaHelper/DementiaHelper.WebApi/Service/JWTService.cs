using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DementiaHelper.WebApi.Service
{
    // adapted from example at https://github.com/SiroccoHub/JwtCore/
    public static class JWTService
    {
        private const string SecretKey = "";
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
            catch (JwtCore.SignatureVerificationException e)
            {
                // TODO: make log trace
                //Console.WriteLine(e.StackTrace);
            }
            return payload;
        }
    }
}

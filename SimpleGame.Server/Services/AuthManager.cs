using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace OneCGame.Server
{
    public class AuthManager
    {
        private ConcurrentDictionary<string, Token> Tokens = new ConcurrentDictionary<string, Token>();
        private static readonly TimeSpan AuthMaxTime = new TimeSpan(12, 0, 0, 0);
        private static readonly string CookiesKeyName = "token";

        public void CreateToken(HttpContext httpContext, Models.User User)
        {
            var guid = Guid.NewGuid().ToString();
            var token = new Token
            {
                User = User,
                AuthDateTime = DateTime.Now
            };

            Tokens.TryAdd(guid, token);

            httpContext.Response.Cookies.Append(CookiesKeyName, guid);
        }

        public bool GetToken(HttpContext httpContext, out Token Token)
        {
            Token = null;

            httpContext.Request.Cookies.TryGetValue(CookiesKeyName, out var tokenName);

            if (tokenName == null)
            {
                return false;
            }

            if (!Tokens.TryGetValue(tokenName, out Token))
            {
                return false;
            }

            if (DateTime.Now - Token.AuthDateTime > AuthMaxTime)
            {
                Tokens.TryRemove(CookiesKeyName, out var _);
                return false;
            }

            Token.AuthDateTime = DateTime.Now;

            return true;
        }
    }
}
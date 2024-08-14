using Microsoft.AspNetCore.Http;
using System;using WebApi.Utils; // Para la clase Cookies
// Agrega la directiva using correspondiente para AuthService

namespace WebApi.Utils
{
    public class Cookies
    {
        public bool SetCookie(string name, string value, HttpResponse response, DateTime? expires = null)
        {
            try
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false, // Set to true in production
                    SameSite = SameSiteMode.Strict,
                    Expires = expires ?? DateTime.UtcNow.AddHours(1)
                };

                response.Cookies.Append(name, value, cookieOptions);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string GetCookie(string name, HttpRequest request)
        {
            return request.Cookies[name];
        }
    }
}
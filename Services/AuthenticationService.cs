using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using WebApi.Context;
using WebApi.Utils;

namespace WebApi.Services
{
    public class AuthenticationService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        private readonly Cookies _cookies;
        private readonly JWT _jwt;

        public AuthenticationService(AppDbContext context, IHttpContextAccessor httpContextAccessor, Cookies cookies, JWT jwt, IConfiguration configuration)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _cookies = cookies;
            _configuration = configuration;
            var jwtKey = _configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key", "JWT key cannot be null");
            _jwt = new JWT(jwtKey);
        }

        public int AuthenticateUser()
        {
            var httpContext = _httpContextAccessor.HttpContext ?? throw new InvalidOperationException("HttpContext is null");
            string cookie = _cookies.GetCookie("token", httpContext.Request);
            if (string.IsNullOrEmpty(cookie))
            {
                throw new UnauthorizedAccessException("Token is missing.");
            }
            string email = _jwt.DecodeToken(cookie);
            if (string.IsNullOrEmpty(email))
            {
                throw new UnauthorizedAccessException("Invalid token.");
            }
            var user = _context.Users.FirstOrDefault(u => u.Email == email) ?? throw new UnauthorizedAccessException("User not found.");

            return user.Id;
        }
        public bool ValidateToken(){
            var httpContext = _httpContextAccessor.HttpContext ?? throw new InvalidOperationException("HttpContext is null");
            string cookie = _cookies.GetCookie("token", httpContext.Request);
            if (string.IsNullOrEmpty(cookie))
            {
                return false;
            }
             string email = _jwt.DecodeToken(cookie);
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }
            var user = _context.Users.FirstOrDefault(u => u.Email == email) ?? throw new UnauthorizedAccessException("User not found.");

            return true;
        }
        
    }
}
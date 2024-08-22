using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.DTOs;

namespace WebApi.Interfaces
{
    public interface IAuthService
    {
        public Task<string> RegisterAsync(RegisterUserDto registerDto);
        public Task<string> LoginAsync(LoginUserDto loginDto, HttpResponse response);
        public string Logout(HttpResponse response);
        public bool ValidateToken();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.DTOs;

namespace WebApi.Interfaces
{
        public interface IAuthRepository
        {
            public Task<User?> GetUserAsync(string Email);
            public Task<string> RegisterAsync(RegisterUserDto user);
            public Task<User?> LoginAsync(LoginUserDto user);
        }
    }
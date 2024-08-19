using WebApi.Models;
using WebApi.DTOs;
using WebApi.Interfaces;
using WebApi.Context;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _context;

        public AuthRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserAsync(string Email)
        {
            try
            {
                User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == Email) ?? throw new Exception("User not found.");
                return user;
            }
            catch (Exception)
            {
                throw new Exception("An error occurred while getting the user.");
            }
        }

        public async Task<User?> LoginAsync(LoginUserDto loginDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                return null;
            }
            return user;
        }

        public async Task<string> RegisterAsync(RegisterUserDto registerDto)
        {
            
            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password)
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return "User registered successfully.";
        }
    }
}
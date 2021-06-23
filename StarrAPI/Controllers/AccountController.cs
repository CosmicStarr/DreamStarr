using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarrAPI.Data;
using StarrAPI.DTOs;
using StarrAPI.InterfacesandClasses;
using StarrAPI.Models;

namespace StarrAPI.Controllers
{
    public class AccountController : BaseAPIController
    {
        private readonly ApplicationDbContext _context;

        private readonly ITokenService _tokenService;

        public AccountController(ApplicationDbContext context, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<TokenDTO>> Register(AppUserDTO appUserDTO)
        {
            if (await UserExist(appUserDTO.Username)) return BadRequest("Username is taken!");
            using var HMAC12 = new HMACSHA512();
            var user = new AppUser
            {
                Username = appUserDTO.Username.ToLower(),
                PasswordHash = HMAC12.ComputeHash(Encoding.UTF8.GetBytes(appUserDTO.Password)),
                PasswordSalt = HMAC12.Key
            };
            _context.Add(user);
            await _context.SaveChangesAsync();
            return new TokenDTO
            {
                Username = user.Username,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenDTO>> Login(LoginDTO loginDTO)
        {
            //Retrieve the Current User.
            var User = await _context.GetAppUsers.FirstOrDefaultAsync(u => u.Username == loginDTO.Username);
            //If User is null, returning Unauthorized.
            if (User == null) return Unauthorized("You're not authorized! Invalid Username");
            //Accessing the Password Algorithm & disposing of it when done with using statement.
            using var HMAC12 = new HMACSHA512(User.PasswordSalt);
            //Retrieving password bytes created by password Algorithm that match password salt.
            var ComputeHash = HMAC12.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));
            //iterating through byte data to find matching Paswword Salt & Hash that was created when the Current User Registered.
            for (int i = 0; i < ComputeHash.Length; i++)
            {
                if (ComputeHash[i] != User.PasswordHash[i]) return Unauthorized("You're not authorized! Invalid Password!");
            }
            return new TokenDTO
            {
                Username = User.Username,
                Token = _tokenService.CreateToken(User)
            };
        }

        private async Task<bool> UserExist(string Username)
        {
            return await _context.GetAppUsers.AnyAsync(u => u.Username == Username.ToLower());
        }
    }
}
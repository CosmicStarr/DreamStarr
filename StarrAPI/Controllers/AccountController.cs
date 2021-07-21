using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;

        }

        [HttpPost("register")]
        public async Task<ActionResult<TokenDTO>> Register(AppUserDTO appUserDTO)
        {
            if (await UserExist(appUserDTO.Username)) return BadRequest("Username is taken!");
            var user = _mapper.Map<AppUser>(appUserDTO);
            // using var HMAC12 = new HMACSHA512();
            user.UserName = appUserDTO.Username.ToLower();
            var results = await _userManager.CreateAsync(user,appUserDTO.Password);
            if(!results.Succeeded) return BadRequest(results.Errors);
            var Members = await _userManager.AddToRoleAsync(user,"Member");
            if(!Members.Succeeded) return BadRequest(Members.Errors);
            // user.PasswordHash = HMAC12.ComputeHash(Encoding.UTF8.GetBytes(appUserDTO.Password));
            // user.PasswordSalt = HMAC12.Key;
            return new TokenDTO
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),
                AlsoknownAs = user.AlsoKnownAs,
                Gender = user.Gender
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenDTO>> Login(LoginDTO loginDTO)
        {
            //Retrieve the Current User.
            var User = await _userManager.Users
            .Include(p => p.Photos)
            .FirstOrDefaultAsync(u => u.UserName == loginDTO.Username.ToLower());
            //If User is null, returning Unauthorized.
            if (User == null) return Unauthorized("You're not authorized! Invalid Username");

            var results = await _signInManager.CheckPasswordSignInAsync(User,loginDTO.Password,false);
            if(!results.Succeeded) return Unauthorized();
            //Accessing the Password Algorithm & disposing of it when done with using statement.
            // using var HMAC12 = new HMACSHA512(User.PasswordSalt);
            //Retrieving password bytes created by password Algorithm that match password salt.
            // var ComputeHash = HMAC12.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));
            //iterating through byte data to find matching Password Salt & Hash that was created when the Current User Registered.
            // for (int i = 0; i < ComputeHash.Length; i++)
            // {
            //     if (ComputeHash[i] != User.PasswordHash[i]) return Unauthorized("You're not authorized! Invalid Password!");
            // }
            return new TokenDTO
            {
                Username = User.UserName,
                Token = await _tokenService.CreateToken(User),
                PhotoUrl = User.Photos.FirstOrDefault(x => x.MainPic)?.PhotoUrl,
                AlsoknownAs = User.AlsoKnownAs,
                Gender = User.Gender
            };
        }

        private async Task<bool> UserExist(string Username)
        {
            return await _userManager.Users.AnyAsync(u => u.UserName == Username.ToLower());
        }
    }
}
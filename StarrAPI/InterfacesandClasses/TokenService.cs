using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StarrAPI.Models;

namespace StarrAPI.InterfacesandClasses
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _Key;
        private readonly UserManager<AppUser> _userManager;
        public TokenService(IConfiguration config, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWTKEY"]));
        }

        public async Task<string> CreateToken(AppUser appUser)
        {
            //Adding Claims. Meaning whoever this Current user Claims to be. In this case the Username+.
            var ClaimsList = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, appUser.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, appUser.UserName),
            };

            var roles = await _userManager.GetRolesAsync(appUser);
            ClaimsList.AddRange(roles.Select(x => new Claim(ClaimTypes.Role,x)));
            //Creating the Credentials for the current User.
            var credentials = new SigningCredentials(_Key, SecurityAlgorithms.HmacSha512Signature);
            //Describing the Token, first, sec, and third part of the token.
            var TokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(ClaimsList),
                Expires = DateTime.Now.AddDays(10),
                SigningCredentials = credentials
            };

            //Creating a token handler which will create the token
            var TokenHandler = new JwtSecurityTokenHandler();
            var Token = TokenHandler.CreateToken(TokenDescription);
            //returning the actually token.
            return TokenHandler.WriteToken(Token);

        }
    }


}
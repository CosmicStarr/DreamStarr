using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StarrAPI.Models;

namespace StarrAPI.InterfacesandClasses
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _Key;
        public TokenService(IConfiguration config)
        {
            _Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWTKEY"]));
        }

        public string CreateToken(AppUser appUser)
        {
            //Adding Claims. Meaning who this Current user Claims to be. In this case the Username.
            var ClaimsList = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, appUser.Username)
            };
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
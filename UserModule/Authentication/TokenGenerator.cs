using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserModule.Dto;

namespace UserModule.Authentication
{
    public class TokenGenerator : TokenGeneratorInterface
    {
        public string GenerateToken(TokenDto dto)
        {
			var tokenHandler = new JwtSecurityTokenHandler();
			var tokenKey = Encoding.UTF8.GetBytes(dto.JwtKey);
			var claims = new List<Claim>()
			{
				   new Claim(JwtRegisteredClaimNames.Sub, dto.UserId.ToString()),
				new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
				new Claim(JwtRegisteredClaimNames.Exp,
					new DateTimeOffset(DateTime.Now.AddMinutes(30)).ToUnixTimeSeconds().ToString()),
				new Claim("username", dto.UserName)
			};
            
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.UtcNow.AddMinutes(10),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return  tokenHandler.WriteToken(token) ;

		}
	}
}

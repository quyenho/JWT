using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JWTApi.Controllers
{
    public class TokenController : Controller
    {

        private const string SECRET_KEY = "this is me at work";
        public static readonly SymmetricSecurityKey SIGNING_KEY = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenController.SECRET_KEY));

        [HttpGet]
        [Route("api/Token/{uid}/{pwd}")]
        public IActionResult Get(string uid, string pwd)
        {
            if (uid == pwd)
            {
                return new ObjectResult(GenerateToken(uid));
            }
            else
                return BadRequest();
        }

        private object GenerateToken(string uid)
        {
            var token = new JwtSecurityToken(
                claims: new Claim[] { new Claim(ClaimTypes.Name, uid) },
                notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                expires: new DateTimeOffset(DateTime.Now.AddMinutes(60)).DateTime,
                signingCredentials:new SigningCredentials(SIGNING_KEY, SecurityAlgorithms.HmacSha256)

                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
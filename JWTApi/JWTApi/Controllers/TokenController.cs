using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JWTApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JWTApi.Controllers
{
    public class TokenController : Controller
    {
        private readonly IConfiguration _configuration; // using DI IOC to populate

        public TokenController(IConfiguration configuration)
        {
            _configuration = configuration;
        }



        // this is bad. Use standard post to get a token.

        //[HttpGet]
        //[Route("api/Token/{uid}/{pwd}")]
        //public IActionResult Get(string uid, string pwd)
        //{
        //    if (uid == pwd)
        //    {
        //        return new ObjectResult(GenerateToken(uid));
        //    }
        //    else
        //        return BadRequest();
        //}



        [AllowAnonymous]
        [HttpPost]
        [Route("api/Token")]
        public IActionResult Token([FromBody] TokenRequest request)
        {

            if (!request.Email.Equals(request.Password))
            {
                return Unauthorized();
            }

            // you would take the request object passed in here and get the user from the db.
            // check if it's null and return unauthorized.
            // instead just hacking a username that would be from the looked up user.
            var userName = "QHo";


            var claims = new[]
            {
                    new Claim(JwtRegisteredClaimNames.Sub, request.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Name, userName),
                    new Claim(ClaimTypes.Email, request.Email)
                };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtAuthentication:SecurityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "localhost:5001",
                audience: "localhost:5001",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30), // expire whenever you want
                signingCredentials: creds);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

        //private object GenerateToken(string uid)
        //{
        //    var token = new JwtSecurityToken(
        //        claims: new Claim[] { new Claim(ClaimTypes.Name, uid) },
        //        notBefore: new DateTimeOffset(DateTime.Now).DateTime,
        //        expires: new DateTimeOffset(DateTime.Now.AddMinutes(60)).DateTime,
        //        signingCredentials: new SigningCredentials(SIGNING_KEY, SecurityAlgorithms.HmacSha256)

        //        );

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}
    }
}
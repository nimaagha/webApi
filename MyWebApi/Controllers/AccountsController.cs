using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyWebApi.Controllers
{
    [ApiVersion("2")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post(string Username, string Password)
        {
            if(true)
            {
                var claims = new List<Claim>
                {
                    new Claim("UserId", Guid.NewGuid().ToString()),
                    new Claim("Name", "Nima Aghaei"),
                };
                string key = "{D2290C00-F4EA-4495-A822-9E21B3FB593F}";
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                var credentials = new SigningCredentials(secretKey,SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    issuer: "bugeto.net",
                    audience:"bugeto.net",
                    expires: DateTime.Now.AddMinutes(5),
                    notBefore: DateTime.Now,
                    claims: claims,
                    signingCredentials: credentials
                    );
                var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(jwtToken);
            }
        }
    }
}

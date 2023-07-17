using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration configuration;
        public AccountsController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

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
                string key = configuration["JwtConfig:Key"];
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                var credentials = new SigningCredentials(secretKey,SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    issuer: configuration["JwtConfig:issuer"],
                    audience: configuration["JwtConfig:audience"],
                    expires: DateTime.Now.AddMinutes(int.Parse(configuration["JwtConfig:expires"])),
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

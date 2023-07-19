﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyWebApi.Models.Helpers;
using MyWebApi.Models.Services;
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
        private readonly UserRepository userRepository;
        private readonly UserTokenRepository userTokenRepository;
        public AccountsController(IConfiguration configuration, UserRepository userRepository, UserTokenRepository userTokenRepository)
        {
            this.configuration = configuration;
            this.userRepository = userRepository;
            this.userTokenRepository = userTokenRepository;
        }

        [HttpPost]
        public IActionResult Post(string Username, string Password)
        {
            SecurityHelper securityHelper = new SecurityHelper();
            if(userRepository.ValidateUser(Username, Password))
            {
                var user = userRepository.GetUser(Guid.Parse("{FB0DB9D6-170E-4023-8C21-2E9920F58AB3}"));
                var claims = new List<Claim>
                {
                    new Claim("UserId", user.Id.ToString()),
                    new Claim("Name", user.Name),
                };
                string key = configuration["JwtConfig:Key"];
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                var credentials = new SigningCredentials(secretKey,SecurityAlgorithms.HmacSha256);
                var tokenExp = DateTime.Now.AddMinutes(int.Parse(configuration["JwtConfig:expires"]));
                var token = new JwtSecurityToken(
                    issuer: configuration["JwtConfig:issuer"],
                    audience: configuration["JwtConfig:audience"],
                    expires: tokenExp,
                    notBefore: DateTime.Now,
                    claims: claims,
                    signingCredentials: credentials
                    );
                var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

                userTokenRepository.SaveToken(new Models.Entities.UserToken()
                {
                    MobileModel = "Iphone Pro max",
                    TokenExp = tokenExp,
                    TokenHash = securityHelper.Getsha256Hash(jwtToken),
                    User = user,
                });
                return Ok(jwtToken);
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}

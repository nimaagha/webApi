using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyWebApi.Models.Dto;
using MyWebApi.Models.Entities;
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
        public IActionResult Post(string PhoneNumber, string SmsCode)
        {
            var loginResult = userRepository.Login(PhoneNumber,SmsCode);
            if(loginResult.IsSuccess == false)
            {
                return Unauthorized(new LoginResultDto()
                {
                    IsSuccess = false,
                    Message = loginResult.Message,
                });
            }
            var token = CreateToken(loginResult.User);
            return Ok(new LoginResultDto()
            {
                IsSuccess = true,
                Data = token,
            });
        }

        [HttpPost]
        [Route("RefreshToken")]
        public IActionResult RefreshToken(string RefreshToken)
        {
            var userToken = userTokenRepository.FindRefreshToken(RefreshToken);
            if(userToken == null)
            {
                return Unauthorized();
            }
            if(userToken.RefreshTokenExp < DateTime.Now)
            {
                return Unauthorized("Token Expired!");
            }
            var token = CreateToken(userToken.User);
            userTokenRepository.DeleteToken(RefreshToken);
            return Ok(token);
        }

        [HttpGet]
        [Route("GetSmsCode")]
        public IActionResult GetSmsCode(string PhoneNumber)
        {
           var smsCode = userRepository.GetCode(PhoneNumber);
            //send code via sms
            return Ok();
        }
        private LoginDataDto CreateToken(User user)
        {
            SecurityHelper securityHelper = new SecurityHelper();
            
                //var user = userRepository.GetUser(Guid.Parse("{FB0DB9D6-170E-4023-8C21-2E9920F58AB3}"));
                var claims = new List<Claim>
                {
                    new Claim("UserId", user.Id.ToString()),
                    new Claim("Name", user?.Name??""),
                };
                string key = configuration["JwtConfig:Key"];
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
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
                var refreshTokn = Guid.NewGuid();

                userTokenRepository.SaveToken(new Models.Entities.UserToken()
                {
                    MobileModel = "Iphone Pro max",
                    TokenExp = tokenExp,
                    TokenHash = securityHelper.Getsha256Hash(jwtToken),
                    User = user,
                    RefreshToken = securityHelper.Getsha256Hash(refreshTokn.ToString()),
                    RefreshTokenExp = DateTime.Now.AddDays(30),
                });

            return new LoginDataDto()
            {
                Token = jwtToken,
                RefreshToken = refreshTokn.ToString(),
            };
        }
    }
}

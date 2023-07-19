using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyWebApi.Models.Services.Validator
{
    public interface ITokenValidator
    {
        Task Execute(TokenValidatedContext context);
    }
    public class TokenValidate : ITokenValidator
    {
        private readonly UserRepository userRepository;
        public TokenValidate(UserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public async Task Execute(TokenValidatedContext context)
        {
            var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
            if(claimsIdentity?.Claims == null || !claimsIdentity.Claims.Any())
            {
                context.Fail("Claims not found!");
                return;
            }
            var userId = claimsIdentity.FindFirst("UserId").Value;
            if(!Guid.TryParse(userId,out Guid userGuid))
            {
                context.Fail("Claims not found!");
                return;
            }
            var user = userRepository.GetUser(userGuid);
            if(user.IsActive == false)
            {
                context.Fail("User is not active!");
                return;
            }
        }
    }
}

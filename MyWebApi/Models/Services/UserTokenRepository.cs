using Microsoft.EntityFrameworkCore;
using MyWebApi.Models.Context;
using MyWebApi.Models.Entities;
using MyWebApi.Models.Helpers;
using System;
using System.Linq;

namespace MyWebApi.Models.Services
{
    public class UserTokenRepository
    {
        private readonly DatabaseContext context;
        public UserTokenRepository(DatabaseContext context)
        {
            this.context = context;
        }

        public void SaveToken(UserToken userToken)
        {
            context.userTokens.Add(userToken);
            context.SaveChanges();
        }

        public UserToken FindRefreshToken(string RefreshToken)
        {
            SecurityHelper securityHelper = new SecurityHelper();
            string RefreshTokenHash = securityHelper.Getsha256Hash(RefreshToken);
            var userToken = context.userTokens.Include(p => p.User).SingleOrDefault(p => p.RefreshToken == RefreshTokenHash);
            return userToken;
        }

        public void DeleteToken(string RefreshToken)
        {
            var token = FindRefreshToken(RefreshToken);
            if (token != null)
            {
                context.userTokens.Remove(token);
                context.SaveChanges();
            }
        }

        public bool CheckExistToken(string token)
        {
            SecurityHelper securityHelper = new SecurityHelper();
            string tokenHash = securityHelper.Getsha256Hash(token);
            var userToken = context.userTokens.Where(p => p.TokenHash == tokenHash).FirstOrDefault();
            return userToken == null ? false : true;
        }
    }
}

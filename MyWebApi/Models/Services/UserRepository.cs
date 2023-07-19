using MyWebApi.Models.Context;
using MyWebApi.Models.Entities;
using System;
using System.Linq;

namespace MyWebApi.Models.Services
{
    public class UserRepository
    {
        private readonly DatabaseContext context;
        public UserRepository(DatabaseContext context)
        {
            this.context = context;
        }

        public User GetUser(Guid Id)
        {
            var user = context.users.SingleOrDefault(p => p.Id == Id);
            return user;
        }
        public bool ValidateUser(string Username, string Password)
        {
            var user = context.users.FirstOrDefault();
            return user != null ? true : false;
        }
    }
}

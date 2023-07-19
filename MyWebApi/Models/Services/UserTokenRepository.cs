using MyWebApi.Models.Context;
using MyWebApi.Models.Entities;

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
    }
}

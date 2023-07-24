using Microsoft.EntityFrameworkCore;
using MyWebApi.Models.Entities;

namespace MyWebApi.Models.Context
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<ToDo> ToDos { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<UserToken> userTokens { get; set; }

        public DbSet<SmsCode> smsCodes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDo>().HasQueryFilter(p => !p.IsRemoved);
        }
    }
}

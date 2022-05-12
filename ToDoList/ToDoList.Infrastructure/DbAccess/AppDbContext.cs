using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ToDoList.Domain.Domain.JWT;
using ToDoList.Domain.Domain.Tasks;
using ToDoList.Domain.Domain.Users;

namespace ToDoList.Infrastructure.DbAccess
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public DbSet<User> Users { get; set; }
        public DbSet<ToDoTask> Tasks { get; set; }
        public DbSet<ToDoTaskList> TaskLists { get; set; }
        public DbSet<UserTokens> UserTokens { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public AppDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(_configuration.GetConnectionString("Default"));
        }
    }
}

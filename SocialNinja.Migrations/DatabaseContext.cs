using Microsoft.EntityFrameworkCore;
using SocialNinja.Contracts.Data;
using SocialNinja.Contracts.Data.Entities;

namespace SocialNinja.Migrations
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Ninja> Ninjas { get; set; }
    }
}
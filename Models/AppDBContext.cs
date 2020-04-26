using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace OidcApp.Models.Entities
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<UserProfile> UserProfiles { get; set; }
    }

    public class UserProfile
    {
        [Key]
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public string OIdProvider { get; set; }
        public string OId { get; set; }
    }
}
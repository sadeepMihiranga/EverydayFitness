using FitnessTracker.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UserService.Models;

namespace UserService.Repositories
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> contextOptions) : base(contextOptions)
        { 
            
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>()             
                .Property(d => d.Status)
                .HasConversion(new EnumToStringConverter<CommonStatusEnum>());
        }
    }
}

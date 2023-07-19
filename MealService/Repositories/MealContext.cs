using FitnessTracker.Enums;
using MealService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MealService.Repositories
{
    public class MealContext : DbContext
    {
        public MealContext(DbContextOptions<MealContext> contextOptions) : base(contextOptions)
        {

        }

        public DbSet<CheatMealType> CheatMealTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<CheatMealType>()
                .Property(d => d.Status)
                .HasConversion(new EnumToStringConverter<CommonStatusEnum>());
        }
    }
}

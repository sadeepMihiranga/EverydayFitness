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
        public DbSet<CheatMealReason> CheatMealReasons { get; set; }
        public DbSet<CheatMeal> CheatMeals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<CheatMealType>()
                .Property(d => d.Status)
                .HasConversion(new EnumToStringConverter<CommonStatusEnum>());

            modelBuilder
               .Entity<CheatMealReason>()
               .Property(d => d.Status)
               .HasConversion(new EnumToStringConverter<CommonStatusEnum>());

            modelBuilder
              .Entity<CheatMeal>()
              .Property(d => d.MealPortionSize)
              .HasConversion(new EnumToStringConverter<MealPortionSizeEnum>());

            modelBuilder
              .Entity<CheatMeal>()
              .Property(d => d.CheatMealSatisfcation)
              .HasConversion(new EnumToStringConverter<CheatMealSatisfcationEnum>());

            modelBuilder
              .Entity<CheatMeal>()
              .Property(d => d.Status)
              .HasConversion(new EnumToStringConverter<CommonStatusEnum>());
        }
    }
}

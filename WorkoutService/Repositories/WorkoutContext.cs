using FitnessTracker.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WorkoutService.Model;

namespace WorkoutService.Repositories
{
    public class WorkoutContext : DbContext
    {
        public WorkoutContext(DbContextOptions<WorkoutContext> contextOptions) : base(contextOptions)
        { 
            
        }

        public DbSet<Workout> Workouts { get; set; }
        public DbSet<WorkoutType> WorkoutTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Workout>()             
                .Property(d => d.Status)
                .HasConversion(new EnumToStringConverter<CommonStatusEnum>());

            modelBuilder
               .Entity<WorkoutType>()
               .Property(d => d.Status)
               .HasConversion(new EnumToStringConverter<CommonStatusEnum>());
        }
    }
}

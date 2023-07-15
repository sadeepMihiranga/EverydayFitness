using Microsoft.EntityFrameworkCore;
using WorkoutService.DTOs;
using WorkoutService.Model;
using WorkoutService.Repositories;

namespace WorkoutService.Services
{
    public class WorkoutServiceImpl : IWorkoutService
    {
        private readonly WorkoutContext _workoutDbContext;

        public WorkoutServiceImpl(WorkoutContext workoutContext)
        {
            _workoutDbContext = workoutContext;
        }

        public async Task<IEnumerable<WorkoutDTO>> GetAllWorkouts()
        {
            if (_workoutDbContext.Workouts == null)
            {
                return Enumerable.Empty<WorkoutDTO>();
            }

            List<Workout> workoutList = await _workoutDbContext.Workouts.Include(w => w.Type).ToListAsync();

            List<WorkoutDTO> newList = workoutList.Select(i => ToWorkoutDTO(i)).ToList();

            return newList;
        }

        public static WorkoutDTO ToWorkoutDTO(Workout workout)
        {
            return new WorkoutDTO()
            {
                Id = workout.Id,
                Type = ToWorkoutTypeDTO(workout.Type),
                Name = workout.Name,
                Date = workout.Date,
                StartTime = workout.StartTime,
                EndTime = workout.EndTime,
                Weight = workout.Weight,
                Reps = workout.Reps,
                Sets = workout.Sets,
                IsRecurring = workout.IsRecurring,
                RecurringType = workout.RecurringType,
                RecurrsionDate = workout.RecurrsionDate,
                Comment = workout.Comment,
                Status = workout.Status,
                User = workout.User,
            };
        }

        public static WorkoutTypeDTO ToWorkoutTypeDTO(WorkoutType workoutType)
        {
            return new WorkoutTypeDTO()
            {
                Id = workoutType.Id,
                Name = workoutType.Name,
                Status = workoutType.Status
            };
        }
    }
}

using WorkoutService.DTOs;

namespace WorkoutService.Services
{
    public interface IWorkoutService
    {
        Task<IEnumerable<WorkoutDTO>> GetAllWorkouts();
    }
}

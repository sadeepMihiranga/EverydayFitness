using WorkoutService.DTOs;

namespace WorkoutService.Services
{
    public interface IWorkoutService
    {
        Task<IEnumerable<WorkoutDTO>> GetAllWorkouts(long userId);

        Task<WorkoutDTO> GetWorkoutById(long userId, long id);

        Task<WorkoutDTO> LogWorkout(long userId, WorkoutDTO workoutDTO);

        Task<IEnumerable<WorkoutDTO>> SearchWorkouts(long userId, string workoutType, int page, int size);

        Task<IEnumerable<WorkoutDTO>> SearchForReport(long userId, string fromDate, string toDate);

        void RemoveWorkout(long userId, long id);
    }
}

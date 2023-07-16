using WorkoutService.DTOs;

namespace WorkoutService.Services
{
    public interface IWorkoutService
    {
        Task<IEnumerable<WorkoutDTO>> GetAllWorkouts(int userId);

        Task<WorkoutDTO> GetWorkoutById(int userId, int id);

        Task<WorkoutDTO> LogWorkout(WorkoutDTO workoutDTO);

        Task<IEnumerable<WorkoutDTO>> SearchWorkouts(int userId, string workoutType, int page, int size);

        Task<IEnumerable<WorkoutDTO>> SearchForReport(int userId, string fromDate, string toDate);

        void RemoveWorkout(int userId, int id);
    }
}

using WorkoutService.DTOs;

namespace WorkoutService.Services
{
    public interface IWorkoutTypeService
    {
        Task<IEnumerable<WorkoutTypeDTO>> GetAllWorkoutTypes();

        Task<WorkoutTypeDTO> GetWorkoutTypeById(int id);
    }
}

using WorkoutService.DTOs;
using WorkoutService.Model;

namespace WorkoutService.Services
{
    public interface IWorkoutTypeService
    {
        Task<IEnumerable<WorkoutTypeDTO>> GetAllWorkoutTypes();

        Task<WorkoutType> ValidateWorkoutType(long id);

        Task<WorkoutTypeDTO> GetWorkoutTypeById(long id);

        WorkoutTypeDTO EntityToDTO(WorkoutType workoutType);
    }
}

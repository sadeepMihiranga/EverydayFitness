using FitnessTracker.Enums;
using Microsoft.EntityFrameworkCore;
using WorkoutService.DTOs;
using WorkoutService.Model;
using WorkoutService.Repositories;

namespace WorkoutService.Services
{
    public class WorkoutTypeServiceImpl : IWorkoutTypeService
    {
        private readonly WorkoutContext _workoutDbContext;

        public WorkoutTypeServiceImpl(WorkoutContext workoutContext)
        {
            _workoutDbContext = workoutContext;
        }

        public async Task<IEnumerable<WorkoutTypeDTO>> GetAllWorkoutTypes()
        {

            if (_workoutDbContext.WorkoutTypes == null)
            {
                return Enumerable.Empty<WorkoutTypeDTO>();
            }

            List<WorkoutType> workoutTypes = await _workoutDbContext.WorkoutTypes
                .Where(w => w.Status == CommonStatusEnum.ACTIVE).ToListAsync();

            return workoutTypes.Select(i => EntityToDTO(i)).ToList();
        }

        public async Task<WorkoutType> ValidateWorkoutType(long id)
        {
            WorkoutType workoutType = await _workoutDbContext.WorkoutTypes
                .Where(w => w.Id == id)
                .Where(w => w.Status == CommonStatusEnum.ACTIVE)
                .FirstOrDefaultAsync();

            if (workoutType == null)
            {
                throw new InvalidDataException("Workout type id is invalid");
            }

            return workoutType;
        }

        public async Task<WorkoutTypeDTO> GetWorkoutTypeById(long id)
        {
            return EntityToDTO(ValidateWorkoutType(id).Result);
        }

        public WorkoutTypeDTO EntityToDTO(WorkoutType workoutType)
        {
            if (workoutType == null)
                return null;

            return new WorkoutTypeDTO()
            {
                Id = workoutType.Id,
                Name = workoutType.Name,
                Status = workoutType.Status
            };
        }
    }
}

using FitnessTracker.Enums;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<IEnumerable<WorkoutDTO>> GetAllWorkouts(int userId)
        {
            if (_workoutDbContext.Workouts == null)
            {
                return Enumerable.Empty<WorkoutDTO>();
            }

            if (userId < 1) 
            {
                throw new InvalidDataException("User id is invalid");
            }

            List<Workout> workoutList = await _workoutDbContext.Workouts
                .Where(w => w.User == userId)
                .Where(w => w.Status == CommonStatusEnum.ACTIVE)
                .Include(w => w.Type).ToListAsync();

            List<WorkoutDTO> workoutDTOs = workoutList.Select(i => EntityToWorkoutDTO(i)).ToList();

            return workoutDTOs;
        }

        public async Task<WorkoutDTO> GetWorkoutById(int userId, int id)
        {
            Workout workout = await _workoutDbContext.Workouts
                .Where(w => w.User == userId)
                .Where(w => w.Id == id)
                .Where(w => w.Status == CommonStatusEnum.ACTIVE)
                .Include(w => w.Type).FirstOrDefaultAsync();

            if (workout == null)
            {
                throw new InvalidDataException("Workout id is invalid");
            }

            return EntityToWorkoutDTO(workout);
        }

        public async Task<WorkoutDTO> LogWorkout(WorkoutDTO workoutDTO)
        {

            WorkoutType workoutType = _workoutDbContext.WorkoutTypes
                .Where(wt => wt.Id == workoutDTO.Type.Id)
                .Where(wt => wt.Status == CommonStatusEnum.ACTIVE)
                .FirstOrDefault();

            if (workoutType == null)
            {
                throw new InvalidDataException("Workout type id is invalid");
            }

            Workout workout = DTOToWorkout(workoutDTO);
            workout.Type = workoutType;
            workout.Status = CommonStatusEnum.ACTIVE;

            _workoutDbContext.Workouts.Add(workout);
            await _workoutDbContext.SaveChangesAsync();

            return EntityToWorkoutDTO(workout);
        }

        public async Task<IEnumerable<WorkoutDTO>> SearchWorkouts(int userId, string workoutType, int page, int size)
        {
            List<Workout> workoutList = await _workoutDbContext.Workouts
                .Where(w => workoutType == null || (workoutType != null && w.Type.Name.StartsWith(workoutType)))
                .Where(w => w.User == userId)
                .Where(w => w.Status == CommonStatusEnum.ACTIVE)
                .Skip(page * size)
                .Take(size)
                .Include(w => w.Type)
                .ToListAsync();

            if (workoutList == null || !workoutList.Any())
            {
                return Enumerable.Empty<WorkoutDTO>();
            }

            List<WorkoutDTO> workoutDTOs = workoutList.Select(i => EntityToWorkoutDTO(i)).ToList();

            return workoutDTOs;
        }

        public async Task<IEnumerable<WorkoutDTO>> SearchForReport(int userId, string fromDate, string toDate)
        {

            List<Workout> workoutList = await _workoutDbContext.Workouts
                .Where(w => w.Date >= Convert.ToDateTime(fromDate))
                .Where(w => w.Date < Convert.ToDateTime(toDate))
                .Where(w => w.Status == CommonStatusEnum.ACTIVE)
                .Include(w => w.Type)
                .ToListAsync();

            if (workoutList == null || !workoutList.Any())
            {
                return Enumerable.Empty<WorkoutDTO>();
            }

            List<WorkoutDTO> workoutDTOs = workoutList.Select(i => EntityToWorkoutDTO(i)).ToList();

            return workoutDTOs;
        }

        public void RemoveWorkout(int userId, int id)
        {
            Workout workout = _workoutDbContext.Workouts
                .Where(w => w.User == userId)
                .Where(w => w.Id == id)
                .Where(w => w.Status == CommonStatusEnum.ACTIVE)
                .Include(w => w.Type).FirstOrDefault();

            if (workout == null)
            {
                throw new InvalidDataException("Workout id is invalid");
            }

            workout.Status = CommonStatusEnum.INACTIVE;

            _workoutDbContext.Entry(workout).State = EntityState.Modified;

            _workoutDbContext.SaveChangesAsync();
        }

        private static Workout DTOToWorkout(WorkoutDTO workoutDTO)
        {
            return new Workout()
            {
                Name = workoutDTO.Name,
                Date = workoutDTO.Date,
                StartTime = workoutDTO.StartTime,
                EndTime = workoutDTO.EndTime,
                Weight = workoutDTO.Weight,
                Reps = workoutDTO.Reps,
                Sets = workoutDTO.Sets,
                IsRecurring = workoutDTO.IsRecurring,
                RecurringType = workoutDTO.RecurringType,
                RecurrsionDate = workoutDTO.RecurrsionDate,
                Comment = workoutDTO.Comment,
                Status = workoutDTO.Status,
                User = workoutDTO.User,
            };
        }

        private static WorkoutDTO EntityToWorkoutDTO(Workout workout)
        {
            return new WorkoutDTO()
            {
                Id = workout.Id,
                Type = WorkoutTypeServiceImpl.EntityToWorkoutTypeDTO(workout.Type),
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
    }
}

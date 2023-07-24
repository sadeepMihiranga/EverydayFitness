using FitnessTracker.Enums;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using WorkoutService.DTOs;
using WorkoutService.Model;
using WorkoutService.Repositories;

namespace WorkoutService.Services
{
    public class WorkoutServiceImpl : IWorkoutService
    {
        private readonly WorkoutContext _workoutDbContext;
        private readonly IWorkoutTypeService _workoutTypeService;
        private readonly IHttpClientFactory _clientFactory;

        public WorkoutServiceImpl(WorkoutContext workoutContext, IWorkoutTypeService workoutTypeService, IHttpClientFactory clientFactory)
        {
            _workoutDbContext = workoutContext;
            _workoutTypeService = workoutTypeService;
            _clientFactory = clientFactory;
        }

        public async Task<IEnumerable<WorkoutDTO>> GetAllWorkouts(long userId)
        {
            if (_workoutDbContext.Workouts == null)
                return Enumerable.Empty<WorkoutDTO>();

            if (userId < 1) 
                throw new InvalidDataException("User id is invalid");

            UserDTO user = ValidateUser(userId).Result;

            if (user == null)
                throw new InvalidDataException("User id is invalid");

            List<Workout> workoutList = await _workoutDbContext.Workouts
                .Where(w => w.User == userId)
                .Where(w => w.Status == CommonStatusEnum.ACTIVE)
                .Include(w => w.Type).ToListAsync();

            return workoutList.Select(i => EntityToDTO(i)).ToList();
        }

        public async Task<WorkoutDTO> GetWorkoutById(long userId, long id)
        {
            Workout workout = await ValidateWorkoutId(userId, id);
            return EntityToDTO(workout);
        }

        public async Task<WorkoutDTO> LogWorkout(long userId, WorkoutDTO workoutDTO)
        {
            WorkoutType workoutType = _workoutTypeService.ValidateWorkoutType(workoutDTO.Type.Id).Result;

            if (workoutType == null)
                throw new InvalidDataException("Workout type id is invalid");

            Workout workout = DTOToEntity(userId, workoutDTO);
            workout.Type = workoutType;
            workout.Status = CommonStatusEnum.ACTIVE;

            _workoutDbContext.Workouts.Add(workout);
            await _workoutDbContext.SaveChangesAsync();

            return EntityToDTO(workout);
        }

        public async Task<IEnumerable<WorkoutDTO>> SearchWorkouts(long userId, string workoutType, int page, int size)
        {
            if (page < 0) throw new InvalidDataException("page is invalid");

            if (size < 0) throw new InvalidDataException("size is invalid");

            List<Workout> workoutList = await _workoutDbContext.Workouts
                .Where(w => workoutType == null || (workoutType != null && w.Type.Name.StartsWith(workoutType)))
                .Where(w => w.User == userId)
                .Where(w => w.Status == CommonStatusEnum.ACTIVE)
                .Skip(page * size)
                .Take(size)
                .Include(w => w.Type)
                .ToListAsync();

            if (workoutList == null || !workoutList.Any())
                return Enumerable.Empty<WorkoutDTO>();

            return workoutList.Select(i => EntityToDTO(i)).ToList();
        }

        public async Task<IEnumerable<WorkoutDTO>> SearchForReport(long userId, string fromDate, string toDate)
        {
            if (String.IsNullOrEmpty(fromDate))
                throw new InvalidDataException("From date is required");

            if (String.IsNullOrEmpty(toDate))
                throw new InvalidDataException("To date is required");

            List<Workout> workoutList = await _workoutDbContext.Workouts
                .Where(w => w.Date >= Convert.ToDateTime(fromDate))
                .Where(w => w.Date < Convert.ToDateTime(toDate + " 23:59:59"))
                .Where(w => w.Status == CommonStatusEnum.ACTIVE)
                .Include(w => w.Type)
                .ToListAsync();

            if (workoutList == null || !workoutList.Any())
            {
                return Enumerable.Empty<WorkoutDTO>();
            }

            return workoutList.Select(i => EntityToDTO(i)).ToList();
        }

        public void RemoveWorkout(long userId, long id)
        {
            Workout workout = ValidateWorkoutId(userId, id).Result;
            workout.Status = CommonStatusEnum.INACTIVE;

            _workoutDbContext.Entry(workout).State = EntityState.Modified;
            _workoutDbContext.SaveChangesAsync();
        }

        public async Task<WorkoutDTO> UpdateWorkout(long userId, long id, WorkoutDTO workoutDTO)
        {
            Workout workout = await ValidateWorkoutId(userId, id);

            WorkoutType workoutType = _workoutTypeService.ValidateWorkoutType(workoutDTO.Type.Id).Result;

            if (workoutType == null)
                throw new InvalidDataException("Workout type id is invalid");

            workout.Name = workoutDTO.Name;
            workout.Date = workoutDTO.Date;
            workout.StartTime = workoutDTO.StartTime;
            workout.EndTime = workoutDTO.EndTime;
            workout.Reps = workoutDTO.Reps;
            workout.Sets = workoutDTO.Sets;
            workout.Type = workoutType;
            workout.IsRecurring = workoutDTO.IsRecurring;
            workout.RecurrsionDate = workoutDTO.RecurrsionDate;
            workout.Comment = workoutDTO.Comment;

            _workoutDbContext.Entry(workout).State = EntityState.Modified;
            await _workoutDbContext.SaveChangesAsync();

            return EntityToDTO(workout);
        }

        private async Task<UserDTO> ValidateUser(long userId)
        {
            // create http client
            HttpClient client = _clientFactory.CreateClient("UserService");

            HttpResponseMessage response = await client.GetAsync(client.BaseAddress.ToString() + "/user/" + userId);

            client.Dispose();

            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // this is the point
                };
                options.Converters.Add(new JsonStringEnumConverter());

                // read response as string
                string responseContent = await response.Content.ReadAsStringAsync();

                // access the root element and extract the specific node from the JSON using the provided path
                JsonElement specificNode = JsonDocument.Parse(responseContent).RootElement.GetProperty("data");

                // deserialize the JSON string to the DTO
                return JsonSerializer.Deserialize<UserDTO>(specificNode.GetRawText(), options);
            }
            else
            {
                return null;
            }
        }

        private async Task<Workout> ValidateWorkoutId(long userId, long id)
        {
            Workout workout = await _workoutDbContext.Workouts
                .Where(w => w.User == userId)
                .Where(w => w.Id == id)
                .Where(w => w.Status == CommonStatusEnum.ACTIVE)
                .Include(w => w.Type).FirstOrDefaultAsync();

            if (workout == null)
                throw new InvalidDataException("Workout id is invalid");

            return workout;
        }

        private static Workout DTOToEntity(long userId, WorkoutDTO workoutDTO)
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
                User = Convert.ToInt32(userId),
            };
        }

        private WorkoutDTO EntityToDTO(Workout workout)
        {
            if (workout == null)
                return null;

            return new WorkoutDTO()
            {
                Id = workout.Id,
                Type = _workoutTypeService.EntityToDTO(workout.Type),
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
                User = ValidateUser(workout.User).Result,
            };
        }
    }
}

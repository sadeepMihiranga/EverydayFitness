using Microsoft.AspNetCore.Mvc;
using WorkoutService.DTOs;
using WorkoutService.DTOs.Wrapper;
using WorkoutService.Services;

namespace WorkoutService.Controllers
{
    [Route("api/workout")]
    [ApiController]
    public class WorkoutController : Controller
    {
        private readonly IWorkoutService _workoutService;

        public WorkoutController(IWorkoutService workoutService)
        {
            _workoutService = workoutService;
        }

        /*[Route("users/{userId}")]
        [HttpGet]
        public async Task<IEnumerable<WorkoutDTO>> GetAll(int userId)
        {
            return await _workoutService.GetAllWorkouts(userId);
        }*/

        [Route("users/{userId}")]
        [HttpGet]
        public async Task<IActionResult> GetAll(int userId)
        {
            return Ok(new Response<IEnumerable<WorkoutDTO>>(await _workoutService.GetAllWorkouts(userId)));
        }

        [Route("users/{userId}/search")]
        [HttpGet]
        public async Task<IActionResult> SearchWorkouts(int userId, string type, int page, int size)
        {
            return Ok(new Response<IEnumerable<WorkoutDTO>>(await _workoutService.SearchWorkouts(userId, type, page, size)));
        }

        [Route("users/{userId}/report")]
        [HttpGet]
        public async Task<IActionResult> SearchForReport(int userId, string fromDate, string toDate)
        {
            return Ok(new Response<IEnumerable<WorkoutDTO>>(await _workoutService.SearchForReport(userId, fromDate, toDate)));
        }

        [Route("users/{userId}/workouts/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetWorkoutById(int userId, int id)
        {
            return Ok(new Response<WorkoutDTO>(await _workoutService.GetWorkoutById(userId, id)));
        }

        [Route("users/{userId}/workouts/{id}")]
        [HttpDelete]
        public void RemoveWorkout(int userId, int id)
        {
            _workoutService.RemoveWorkout(userId, id);
        }

        [Route("users/{userId}")]
        [HttpPost]
        public async Task<IActionResult> LogWorkout(WorkoutDTO workoutDTO)
        {
            return Ok(new Response<WorkoutDTO>(await _workoutService.LogWorkout(workoutDTO)));
        }
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkoutService.DTOs;
using WorkoutService.Model;
using WorkoutService.Repositories;
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

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IEnumerable<WorkoutDTO>> GetAll()
        {
            return await _workoutService.GetAllWorkouts();
        }
    }
}

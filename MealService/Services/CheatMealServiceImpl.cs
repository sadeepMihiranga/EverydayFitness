using FitnessTracker.Enums;
using MealService.DTOs;
using MealService.Models;
using MealService.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MealService.Services
{
    public class CheatMealServiceImpl : ICheatMealService
    {
        private readonly MealContext _mealDbContext;
        private readonly ICheatMealReasonService _cheatMealReasonService;
        private readonly ICheatMealTypeService _cheatMealTypeService;
        private readonly IHttpClientFactory _clientFactory;

        public CheatMealServiceImpl(MealContext mealContext, 
            ICheatMealReasonService cheatMealReasonService, 
            ICheatMealTypeService cheatMealTypeService,
            IHttpClientFactory clientFactory)
        {
            _mealDbContext = mealContext;
            _cheatMealReasonService = cheatMealReasonService;
            _cheatMealTypeService = cheatMealTypeService;
            _clientFactory = clientFactory;
        }

        public async Task<IEnumerable<CheatMealDTO>> GetAllCheatMeals(long userId)
        {
            if (_mealDbContext.CheatMeals == null)
            {
                return Enumerable.Empty<CheatMealDTO>();
            }

            if (userId < 1)
            {
                throw new InvalidDataException("User id is invalid");
            }

            List<CheatMeal> cheatMealList = await _mealDbContext.CheatMeals
                .Where(c => c.User == userId)
                .Where(c => c.Status == CommonStatusEnum.ACTIVE)
                .Include(c => c.MealType)
                .Include(c => c.MealReason).ToListAsync();

            return cheatMealList.Select(i => EntityToDTO(i)).ToList();
        }

        public async Task<CheatMealDTO> GetCheatMealById(long userId, long id)
        {
            return EntityToDTO(ValidateCheatMeal(userId, id).Result);
        }

        public async Task<CheatMealDTO> LogCheatMeal(CheatMealDTO cheatMealDTO)
        {
            CheatMealType cheatMealType = _cheatMealTypeService.ValidateCheatMealType(cheatMealDTO.MealType.Id).Result;
            CheatMealReason cheatMealReason = _cheatMealReasonService.ValidateCheatMealReason(cheatMealDTO.MealReason.Id).Result;
             
            CheatMeal cheatMeal = DTOToEntity(cheatMealDTO);
            cheatMeal.MealType = cheatMealType;
            cheatMeal.MealReason = cheatMealReason;
            cheatMeal.Status = CommonStatusEnum.ACTIVE;

            _mealDbContext.CheatMeals.Add(cheatMeal);
            await _mealDbContext.SaveChangesAsync();

            return EntityToDTO(cheatMeal);
        }

        public void RemoveCheatMeal(long userId, long id)
        {
            CheatMeal cheatMeal = ValidateCheatMeal(userId, id).Result;

            cheatMeal.Status = CommonStatusEnum.INACTIVE;

            _mealDbContext.Entry(cheatMeal).State = EntityState.Modified;
            _mealDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<CheatMealDTO>> SearchCheatMeals(long userId, string mealType, int page, int size)
        {
            List<CheatMeal> cheatMealList = await _mealDbContext.CheatMeals
                 .Where(c => mealType == null || (mealType != null && c.MealType.Name.StartsWith(mealType)))
                 .Where(c => c.User == userId)
                 .Where(c => c.Status == CommonStatusEnum.ACTIVE)
                 .Skip(page * size)
                 .Take(size)
                 .Include(c => c.MealType)
                 .Include(c => c.MealReason)
                 .ToListAsync();

            if (cheatMealList == null || !cheatMealList.Any())
            {
                return Enumerable.Empty<CheatMealDTO>();
            }

            return cheatMealList.Select(i => EntityToDTO(i)).ToList();
        }

        public async Task<IEnumerable<CheatMealDTO>> SearchForReport(long userId, string fromDate, string toDate)
        {
            if (String.IsNullOrEmpty(fromDate))
            {
                throw new InvalidDataException("From date is required");
            }

            if (String.IsNullOrEmpty(toDate))
            {
                throw new InvalidDataException("To date is required");
            }

            List<CheatMeal> cheatMealList = await _mealDbContext.CheatMeals
                .Where(c => c.DateTimeTaken >= Convert.ToDateTime(fromDate))
                .Where(c => c.DateTimeTaken <= Convert.ToDateTime(toDate + " 23:59:59"))
                .Where(c => c.Status == CommonStatusEnum.ACTIVE)
                .Include(c => c.MealType)
                .Include(c => c.MealReason)
                .ToListAsync();

            if (cheatMealList == null || !cheatMealList.Any())
            {
                return Enumerable.Empty<CheatMealDTO>();
            }

            return cheatMealList.Select(i => EntityToDTO(i)).ToList();
        }

        private async Task<CheatMeal> ValidateCheatMeal(long userId, long id)
        {
            CheatMeal cheatMeal = await _mealDbContext.CheatMeals
               .Where(c => c.User == userId)
               .Where(c => c.Id == id)
               .Where(c => c.Status == CommonStatusEnum.ACTIVE)
               .Include(c => c.MealType)
               .Include(c => c.MealReason).FirstOrDefaultAsync();

            if (cheatMeal == null)
            {
                throw new InvalidDataException("Cheat meal id is invalid");
            }

            return cheatMeal;
        }

        private CheatMeal DTOToEntity(CheatMealDTO cheatMealDTO)
        {
            return new CheatMeal()
            {
                Name = cheatMealDTO.Name,
                DateTimeTaken = cheatMealDTO.DateTimeTaken,
                CaloriesTaken = cheatMealDTO.CaloriesTaken,
                CheatMealSatisfcation = cheatMealDTO.CheatMealSatisfcation,
                MealPortionSize = cheatMealDTO.MealPortionSize,
                Comment = cheatMealDTO.Comment,
                User = cheatMealDTO.User,
                Status = cheatMealDTO.Status               
            };
        }

        private CheatMealDTO EntityToDTO(CheatMeal cheatMeal)
        {
            return new CheatMealDTO()
            {
                Id = cheatMeal.Id,
                Name = cheatMeal.Name,
                DateTimeTaken = cheatMeal.DateTimeTaken,
                CaloriesTaken = cheatMeal.CaloriesTaken,
                CheatMealSatisfcation = cheatMeal.CheatMealSatisfcation,
                MealPortionSize = cheatMeal.MealPortionSize,
                Comment = cheatMeal.Comment,
                User = cheatMeal.User,
                Status = cheatMeal.Status,
                MealReason = _cheatMealReasonService.EntityToDTO(cheatMeal.MealReason),
                MealType = _cheatMealTypeService.EntityToDTO(cheatMeal.MealType),               
            };
        }

        private async Task<UserDTO> ValidateUser(long userId)
        {
            // create http client
            HttpClient client = _clientFactory.CreateClient("UserService");

            HttpResponseMessage response = await client.GetAsync(client.BaseAddress.ToString() + "/user/" + userId);

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
    }
}

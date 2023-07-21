using FitnessTracker.Enums;
using MealService.DTOs;
using MealService.Models;
using MealService.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MealService.Services
{
    public class CheatMealReasonServiceImpl : ICheatMealReasonService
    {
        private readonly MealContext _mealDbContext;

        public CheatMealReasonServiceImpl(MealContext mealContext)
        {
            _mealDbContext = mealContext;
        }

        public async Task<IEnumerable<CheatMealReasonDTO>> GetAllCheatMealReasons()
        {
            if (_mealDbContext.CheatMealReasons == null)
            {
                return Enumerable.Empty<CheatMealReasonDTO>();
            }

            List<CheatMealReason> cheatMealReasons = await _mealDbContext.CheatMealReasons
                .Where(w => w.Status == CommonStatusEnum.ACTIVE).ToListAsync();

            return cheatMealReasons.Select(i => EntityToDTO(i)).ToList();
        }

        public async Task<CheatMealReasonDTO> GetCheatMealReasonById(long id)
        {
            return EntityToDTO(ValidateCheatMealReason(id).Result);
        }

        public async Task<CheatMealReason> ValidateCheatMealReason(long id)
        {
            CheatMealReason cheatMealReason = await _mealDbContext.CheatMealReasons
                .Where(w => w.Id == id)
                .Where(w => w.Status == CommonStatusEnum.ACTIVE)
                .FirstOrDefaultAsync();

            if (cheatMealReason == null)
            {
                throw new InvalidDataException("Cheat meal reason id is invalid");
            }

            return cheatMealReason;
        }

        public CheatMealReasonDTO EntityToDTO(CheatMealReason cheatMealReason)
        {
            if (cheatMealReason == null)
                return null;

            return new CheatMealReasonDTO()
            {
                Id = cheatMealReason.Id,
                Name = cheatMealReason.Name,
                Status = cheatMealReason.Status
            };
        }
    }
}

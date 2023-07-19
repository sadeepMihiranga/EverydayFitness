using FitnessTracker.Enums;
using MealService.DTOs;
using MealService.Models;
using MealService.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MealService.Services
{
    public class CheatMealTypeServiceImpl : ICheatMealTypeService
    {
        private readonly MealContext _mealDbContext;

        public CheatMealTypeServiceImpl(MealContext mealContext)
        {
            _mealDbContext = mealContext;
        }

        public async Task<IEnumerable<CheatMealTypeDTO>> GetAllCheatMealTypes()
        {
            if (_mealDbContext.CheatMealTypes == null)
            {
                return Enumerable.Empty<CheatMealTypeDTO>();
            }

            List<CheatMealType> cheatMealTypes = await _mealDbContext.CheatMealTypes
                .Where(w => w.Status == CommonStatusEnum.ACTIVE).ToListAsync();

            List<CheatMealTypeDTO> cheatMealTypeDTOList = cheatMealTypes.Select(i => EntityToDTO(i)).ToList();

            return cheatMealTypeDTOList;
        }

        public async Task<CheatMealTypeDTO> GetCheatMealTypeById(int id)
        {
            CheatMealType cheatMealType = await _mealDbContext.CheatMealTypes
                .Where(w => w.Id == id)
                .Where(w => w.Status == CommonStatusEnum.ACTIVE)
                .FirstOrDefaultAsync();

            if (cheatMealType == null)
            {
                throw new InvalidDataException("Cheat meal type id is invalid");
            }

            return EntityToDTO(cheatMealType);
        }

        public static CheatMealTypeDTO EntityToDTO(CheatMealType cheatMealType)
        {
            if (cheatMealType == null)
                return null;

            return new CheatMealTypeDTO()
            {
                Id = cheatMealType.Id,
                Name = cheatMealType.Name,
                Status = cheatMealType.Status
            };
        }
    }
}

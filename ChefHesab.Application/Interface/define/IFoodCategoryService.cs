using ChefHesab.Domain;
using ChefHesab.Dto.define.FoodStuff;
using static ChefHesab.Dto.define.FoodStuff.SnapFoodStufCategory;

namespace ChefHesab.Application.Interface.define
{
    public interface IFoodCategoryService
    {
        Task<bool> AddFoodCategory(Rootobject model);
        List<FoodCategory> GetAll();
    }
}
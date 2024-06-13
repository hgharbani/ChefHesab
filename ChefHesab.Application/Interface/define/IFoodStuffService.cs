using ChefHesab.Dto.define.FoodStuff;

namespace ChefHesab.Application.Interface.define
{
    public interface IFoodStuffService
    {
        Task<bool> AddFoodStuffFromSnap(SnapFoodStufCategory.Rootobject model, long categoryId);
        Task<List<FoodStuffVM>> GetFoodStuff();
    }
}
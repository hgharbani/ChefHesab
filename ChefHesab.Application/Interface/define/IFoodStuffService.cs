using ChefHesab.Dto.define.FoodStuff;
using ChefHesab.Share.model;

namespace ChefHesab.Application.Interface.define
{
    public interface IFoodStuffService
    {
        Task<ChefResult> Add(CreateFoodStuffVM model);
        Task<bool> AddFoodStuffFromSnap(SnapFoodStufCategory.Rootobject model, long categoryId);
        Task<ChefResult> Edit(CreateFoodStuffVM model);
        Task<ChefResult> GetExcelFileAndSaveInSql();
        Task<List<FoodStuffVM>> GetFoodStuff();
        dynamic GetFoodStuffPriceWithCompany(FoodStuffSearch request);
        dynamic GetFoodStuffWithCategory(FoodfSearch request);
        Task<CreateFoodStuffVM> GetFoodStuffForEdit(Guid id);
    }
}
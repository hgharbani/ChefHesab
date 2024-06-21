using ChefHesab.Domain;
using ChefHesab.Dto.define.FoodCategory;
using ChefHesab.Dto.define.FoodStuff;
using ChefHesab.Share.model.KendoModel;
using static ChefHesab.Dto.define.FoodStuff.SnapFoodStufCategory;

namespace ChefHesab.Application.Interface.define
{
    public interface IFoodCategoryService
    {
     
        dynamic GetAllByKendoFilter(SearchFoodCategoryVM request);
    }
}
using ChefHesab.Domain;
using ChefHesab.Dto.define.AdditionalCost;
using ChefHesab.Dto.define.FoodStuff;
using ChefHesab.Share.model;

namespace ChefHesab.Application.Interface.define
{
    public interface IAdditionalCostService
    {
        Task<ChefResult> Add(CreateAdditionalCostVM additionalCost);
        Task<ChefResult> Delete(CreateAdditionalCostVM additionalCost);
        Task<ChefResult> Edit(CreateAdditionalCostVM additionalCost);
        dynamic GetAdditionalCostWithCompany(AdditionalCostSearchModel request);
        CreateAdditionalCostVM GetForEdit(Guid id);


    }
}
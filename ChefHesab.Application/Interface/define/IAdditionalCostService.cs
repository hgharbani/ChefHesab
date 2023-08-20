using ChefHesab.Domain;

namespace ChefHesab.Application.Interface.define
{
    public interface IAdditionalCostService
    {
        List<AdditionalCost> GetAdditionalCost();
    }
}
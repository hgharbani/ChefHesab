using ChefHesab.Domain;
using ChefHesab.Dto.define.AdditionalCost;

namespace ChefHesab.Application.Interface.define
{
    public interface IAdditionalCostService
    {
        List<AdditionalCost> GetAdditionalCost();
        Task<Tuple<List<AdditionalCostVM>, int>> GetAdditionalCostListWithPeginition(AdditionalCostVM additionalCost);
    }
}
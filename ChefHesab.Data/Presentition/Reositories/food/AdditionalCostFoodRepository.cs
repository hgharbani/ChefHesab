using ChefHesab.Data;
using ChefHesab.Data.Presentition.Context;
using ChefHesab.Data.Presentition.Reositories.generic;
using ChefHesab.Domain.Peresentition.IRepositories.food;
using Microsoft.EntityFrameworkCore;

namespace ChefHesab.Domain.Peresentition.IRepositories
{
    public class AdditionalCostFoodRepository : GenericRepository<AdditionalCostFood>, IAdditionalCostFoodRepository
    {
         
        public AdditionalCostFoodRepository(ChefHesabContext chefHesab) : base(chefHesab    )
        { 
        }



    }
}
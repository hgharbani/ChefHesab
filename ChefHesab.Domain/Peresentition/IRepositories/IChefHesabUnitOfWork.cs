using ChefHesab.Domain.Peresentition.IRepositories.define;
using ChefHesab.Domain.Peresentition.IRepositories.food;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Domain.Peresentition.IRepositories
{
    public interface IChefHesabUnitOfWork : IBaseUnitOfWork
    {
        IPersonalRepository PersonalRepository { get; }
        IAdditionalCostRepository AdditionalCostRepository { get; }
        IContractingCompanyRepository ContractingCompanyRepository { get; }
        IFoodCategoryRepository FoodCategoryRepository { get; }
        IFoodStuffRepository FoodStuffRepository { get; }
        IStuffPriceRepository StuffPriceRepository { get; }
        IIngredinsFoodRepository IngredinsFoodRepository { get; }
        IAuthenticateRepository AuthenticateRepository { get; }
        IAdditionalCostFoodRepository AdditionalCostFoodRepository { get; }
        IFoodProviderRepository FoodProviderRepository { get; }
    }
    
}

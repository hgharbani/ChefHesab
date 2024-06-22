using ChefHesab.Data.Presentition.Context;
using ChefHesab.Data.Presentition.Reositories.define;
using ChefHesab.Domain.Peresentition.IRepositories;
using ChefHesab.Domain.Peresentition.IRepositories.define;
using ChefHesab.Domain.Peresentition.IRepositories.food;

namespace ChefHesab.Data.Presentition.Reositories
{
    public class ChefHesabUnitOfWork :  UnitOfWork<ChefHesabContext>, IChefHesabUnitOfWork
    {
        public ChefHesabUnitOfWork(ChefHesabContext chefHesab) : base(chefHesab)
        {
            ChefHesab = chefHesab;
        }
        private IPersonalRepository? personalRepository;
        public IPersonalRepository PersonalRepository
        {
            get
            {
                personalRepository ??= new PersonalRepository(ChefHesab);
                return personalRepository;
            }
        }
        private IAdditionalCostRepository? additionalCostRepository;
        public IAdditionalCostRepository AdditionalCostRepository
        {
            get
            {
                additionalCostRepository ??= new AdditionalCostRepository(ChefHesab);
                return additionalCostRepository;
            }
        }
        private IContractingCompanyRepository? contractingCompanyRepository;
        public IContractingCompanyRepository ContractingCompanyRepository
        {
            get
            {
                contractingCompanyRepository ??= new ContractingCompanyRepository(ChefHesab);
                return contractingCompanyRepository;
            }
        }

        private IFoodCategoryRepository? foodCategoryRepository;
        public IFoodCategoryRepository FoodCategoryRepository
        {
            get
            {
                foodCategoryRepository ??= new FoodCategoryRepository(ChefHesab);
                return foodCategoryRepository;
            }
        }
        private IFoodStuffRepository? foodStuffRepository;
        public IFoodStuffRepository FoodStuffRepository
        {
            get
            {
                foodStuffRepository ??= new FoodStuffRepository(ChefHesab);
                return foodStuffRepository;
            }
        }
        private IAdditionalCostFoodRepository? additionalCostFoodRepository;
        public IAdditionalCostFoodRepository AdditionalCostFoodRepository
        {
            get
            {
                additionalCostFoodRepository ??= new AdditionalCostFoodRepository(ChefHesab);
                return additionalCostFoodRepository;
            }
        }
        private IFoodProviderRepository? foodProviderRepository;
        public IFoodProviderRepository FoodProviderRepository
        {
            get
            {
                foodProviderRepository ??= new FoodProviderRepository(ChefHesab);
                return foodProviderRepository;
            }
        }
        private IIngredinsFoodRepository? ingredinsFoodRepository;
        public IIngredinsFoodRepository IngredinsFoodRepository
        {
            get
            {
                ingredinsFoodRepository ??= new IngredinsFoodRepository(ChefHesab);
                return ingredinsFoodRepository;
            }
        }
        private IStuffPriceRepository? stuffPriceRepository;
        public IStuffPriceRepository StuffPriceRepository
        {
            get
            {
                stuffPriceRepository ??= new StuffPriceRepository(ChefHesab);
                return stuffPriceRepository;
            }
        }
        private IAuthenticateRepository? authenticateRepository;
        public IAuthenticateRepository AuthenticateRepository
        {
            get
            {
                authenticateRepository ??= new AuthenticateRepository(ChefHesab);
                return authenticateRepository;
            }
        }

        public ChefHesabContext ChefHesab { get; }
    }
}

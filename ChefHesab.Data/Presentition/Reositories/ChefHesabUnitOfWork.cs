using ChefHesab.Data.Presentition.Context;
using ChefHesab.Data.Presentition.Reositories.define;
using ChefHesab.Domain.Peresentition.IRepositories;
using ChefHesab.Domain.Peresentition.IRepositories.define;
using ChefHesab.Domain.Peresentition.IRepositories.food;

namespace ChefHesab.Data.Presentition.Reositories
{
    public class ChefHesabUnitOfWork :  UnitOfWork<ChefHesabContext>, IChefHesabUnitOfWork
    {

        public ChefHesabUnitOfWork(ChefHesabContext context) : base(context)
        {
           
        }
        private IPersonalRepository? personalRepository;
        public IPersonalRepository PersonalRepository
        {
            get
            {
                personalRepository ??= new PersonalRepository(DatabaseContext);
                return personalRepository;
            }
        }
        private IAdditionalCostRepository? additionalCostRepository;
        public IAdditionalCostRepository AdditionalCostRepository
        {
            get
            {
                additionalCostRepository ??= new AdditionalCostRepository(DatabaseContext);
                return additionalCostRepository;
            }
        }
        private IContractingCompanyRepository? contractingCompanyRepository;
        public IContractingCompanyRepository ContractingCompanyRepository
        {
            get
            {
                contractingCompanyRepository ??= new ContractingCompanyRepository(DatabaseContext);
                return contractingCompanyRepository;
            }
        }

        private IFoodCategoryRepository? foodCategoryRepository;
        public IFoodCategoryRepository FoodCategoryRepository
        {
            get
            {
                foodCategoryRepository ??= new FoodCategoryRepository(DatabaseContext);
                return foodCategoryRepository;
            }
        }
        private IFoodStuffRepository? foodStuffRepository;
        public IFoodStuffRepository FoodStuffRepository
        {
            get
            {
                foodStuffRepository ??= new FoodStuffRepository(DatabaseContext);
                return foodStuffRepository;
            }
        }
        private IAdditionalCostFoodRepository? additionalCostFoodRepository;
        public IAdditionalCostFoodRepository AdditionalCostFoodRepository
        {
            get
            {
                additionalCostFoodRepository ??= new AdditionalCostFoodRepository(DatabaseContext);
                return additionalCostFoodRepository;
            }
        }
        private IFoodProviderRepository? foodProviderRepository;
        public IFoodProviderRepository FoodProviderRepository
        {
            get
            {
                foodProviderRepository ??= new FoodProviderRepository(DatabaseContext);
                return foodProviderRepository;
            }
        }
        private IIngredinsFoodRepository? ingredinsFoodRepository;
        public IIngredinsFoodRepository IngredinsFoodRepository
        {
            get
            {
                ingredinsFoodRepository ??= new IngredinsFoodRepository(DatabaseContext);
                return ingredinsFoodRepository;
            }
        }
        private IStuffPriceRepository? stuffPriceRepository;
        public IStuffPriceRepository StuffPriceRepository
        {
            get
            {
                stuffPriceRepository ??= new StuffPriceRepository(DatabaseContext);
                return stuffPriceRepository;
            }
        }
        private IAuthenticateRepository? authenticateRepository;
        public IAuthenticateRepository AuthenticateRepository
        {
            get
            {
                authenticateRepository ??= new AuthenticateRepository(DatabaseContext);
                return authenticateRepository;
            }
        }

       

    }
}

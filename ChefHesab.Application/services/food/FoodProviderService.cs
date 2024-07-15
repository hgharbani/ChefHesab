using AutoMapper;
using ChefHesab.Application.Interface.food;
using ChefHesab.Dto.define.FoodStuff;
using ChefHesab.Dto.food.FoodProvider;
using ChefHesab.Share.model;
using ChefHesab.Share.model.KendoModel.Response;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Linq.Dynamic;
using ChefHesab.Share.Extiontions.KendoExtentions;




namespace ChefHesab.Domain.Peresentition.IRepositories.food
{
    /// <summary>
    /// غذاهای ارائه شده به شرکت
    /// </summary>
    public class FoodProviderService : IFoodProviderService
    {
        public IChefHesabUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public FoodProviderService(IChefHesabUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        private bool IsDuplicate(CreateFoodProviderVM foodProvider)
        {
            if (!string.IsNullOrEmpty(foodProvider.Id.ToString()))
            {
                return _unitOfWork.FoodProviderRepository
                    .Any(a => a.Id != foodProvider.Id
                    && a.ContractCompanyId == foodProvider.ContractCompanyId
                    && a.FoodStuffId == foodProvider.FoodStuffId);

            }
            else
            {
                return _unitOfWork.FoodProviderRepository
                    .Any(a => a.ContractCompanyId == foodProvider.ContractCompanyId
                    && a.FoodStuffId == foodProvider.FoodStuffId);
            }

        }
        public dynamic GetFoodProviders(SearchfoodProvider request)
        {
            var foodproviderQuery = _unitOfWork
                .FoodProviderRepository
                .SelectQuery()
                .Include(a => a.FoodStuff)
                .ThenInclude(a => a.FoodCategory)
                .Include(a => a.ContractingCompany).AsNoTracking();
            if (request.CompanyId.HasValue)
            {

                foodproviderQuery = foodproviderQuery.Where(a => a.ContractCompanyId == request.CompanyId.Value)
                    ;
            }
            if (request.FoodCategoryId.HasValue && request.FoodCategoryId.Value>0)
            {

                foodproviderQuery = foodproviderQuery.Where(a => a.FoodStuff.FoodCategoryId == request.FoodCategoryId.Value)
                    ;
            }
            int total = foodproviderQuery.Count();
            IList resultData;
            bool isGrouped = false;
            var aggregates = new Dictionary<string, Dictionary<string, string>>();

            var data = foodproviderQuery.Select(a => new FoodProviderVM()
            {
                Id = a.Id,
                ContractCompanyId = a.ContractCompanyId,
                FoodStuffId = a.FoodStuffId,
                CategoryName = a.FoodStuff.FoodCategory.Title,
                CompanyName = a.ContractingCompany.CompanyName,
                FoodName = a.FoodStuff.Title,
                AmountRequested = a.AmountRequested ?? 0,

            });

            if (request.Sorts != null)
            {
                data = data.Sort(request.Sorts);
            }

            if (request.Filter != null)
            {
                data = data.Filter(request.Filter);
                total = data.Count();
            }

            if (request.Aggregates != null)
            {
                aggregates = data.CalculateAggregates(request.Aggregates);
            }

            if (request.Take > 0)
            {
                data = data.Page(request.Skip, request.Take);
            }

            if (request.Groups != null && request.Groups.Count > 0 && !request.GroupPaging)
            {
                resultData = data.Group(request.Groups).Cast<Group>().ToList();
                isGrouped = true;
            }
            else
            {
                resultData = data.ToList();
            }

            var result = new Response(resultData, aggregates, total, isGrouped).ToResult();
            return result;
        }
        public async Task<CreateFoodProviderVM> GetForEdit(Guid id)
        {
            var result = _unitOfWork.FoodProviderRepository
                .Where(a => a.Id == id)
                .Select(a => _mapper.Map<FoodProvider, CreateFoodProviderVM>(a))
                .FirstOrDefault();
            return result;
        }

        public async Task<FoodProviderVM> GetHeaderAnalyzeFood(Guid id)
        {
            try
            {
                var result = await _unitOfWork.FoodProviderRepository.GetAllAsNoTracking()
               .Include(a => a.FoodStuff).ThenInclude(a => a.FoodCategory)
               .Include(a=>a.ContractingCompany)
               .Where(a => a.Id == id)
               .FirstOrDefaultAsync();

                var model = new FoodProviderVM
                {
                    Id = result.Id,
                    CategoryName = result.FoodStuff.FoodCategory.Title,
                    CompanyName = result.ContractingCompany.CompanyName,
                    AmountRequested = result.AmountRequested.HasValue ? result.AmountRequested.Value : 0,
                    FoodName = result.FoodStuff.Title,
                    ContractCompanyId=result.ContractCompanyId
                };
                return model;
            }
            catch(Exception ex)
            {
                return new FoodProviderVM();
            }
           
        }

        public async Task<ChefResult> Add(CreateFoodProviderVM model)
        {
            var result = new ChefResult();
            try
            {

                if (IsDuplicate(model))
                {
                    result.AddError("داده وارد شده تکراری می باشد");
                    return result;
                }
                model.Id = Guid.NewGuid();
                var mapper = _mapper.Map<FoodProvider>(model);

                _unitOfWork.FoodProviderRepository.Add(mapper);
                var id = await _unitOfWork.SaveAsync();


                return result;
            }
            catch (Exception ex)
            {
                result.AddError("عملیات نا موفق بود");
                return result;
            }

        }
        public async Task<ChefResult> Edit(CreateFoodProviderVM model)
        {
            var result = new ChefResult();
            try
            {

                if (IsDuplicate(model))
                {
                    result.AddError("داده وارد شده تکراری می باشد");
                    return result;
                }
                var find = _unitOfWork.FoodProviderRepository.Select(a => a.Id == model.Id);
                if (find == null)
                {
                    result.AddError("داده مورد نظر حذف شده است");
                    return result;
                }

                var mapper = _mapper.Map<CreateFoodProviderVM, FoodProvider>(model, find);
                _unitOfWork.FoodProviderRepository.Update(mapper);
                await _unitOfWork.SaveAsync();
                return result;
            }
            catch (Exception ex)
            {
                result.AddError("عملیات نا موفق بود");
                return result;
            }

        }

    }
}

using AutoMapper;
using ChefHesab.Application.Interface.food;
using ChefHesab.Dto.food.IngredinsFood;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using ChefHesab.Share.Extiontions.KendoExtentions;
using ChefHesab.Share.model.KendoModel.Response;
using ChefHesab.Share.model;
using static ChefHesab.Dto.define.FoodStuff.SnapFoodStufCategory;
using ChefHesab.Dto.define.ContractingCompany;

namespace ChefHesab.Domain.Peresentition.IRepositories.food
{
    /// <summary>
    /// سرویس مدیریت مواد لازم هر پرس غذا
    /// </summary>
    public class IngredinsFoodService : IIngredinsFoodService
    {
        public IChefHesabUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public IngredinsFoodService(IChefHesabUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        private bool IsDuplicate(CreateIngredinsFoodVM model)
        {
            if (!string.IsNullOrEmpty(model.Id.ToString()))
            {
                return _unitOfWork.IngredinsFoodRepository
                    .Any(a => a.Id != model.Id
                    && a.FoodProviderId == model.FoodProviderId
                    && a.StuffPriceId == model.StuffPriceId);

            }
            else
            {
                return _unitOfWork.IngredinsFoodRepository
                    .Any(a => a.FoodProviderId == model.FoodProviderId
                    && a.StuffPriceId == model.StuffPriceId);
            }

        }
        public dynamic GetIngredinsFoods(SearchIngredinsFoodVm request)
        {
            var foodproviderQuery = _unitOfWork
                .IngredinsFoodRepository
                .SelectQuery().Where(a => a.FoodProviderId == request.FoodProviderId)
                .Include(a => a.StuffPrice).ThenInclude(a => a.FoodStuff)
                .ThenInclude(a => a.FoodCategory)
                .AsNoTracking();

            int total = foodproviderQuery.Count();
            IList resultData;
            bool isGrouped = false;
            var aggregates = new Dictionary<string, Dictionary<string, string>>();

            var data = foodproviderQuery.Select(a => new IngredinsFoodVM()
            {
                Id = a.Id,
                StuffPriceTitle = a.StuffPrice.FoodStuff.Title,
                StuffPriceId = a.StuffPriceId,
                Amount = a.Amount,
                Cost = a.Cost,
                Price = a.StuffPrice.TotalPrice,
                Unit = a.Unit,
                CategoryTitle= a.StuffPrice.FoodStuff.FoodCategory.Title,

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


        public async Task<ChefResult> Create(CreateIngredinsFoodVM model)
        {
            var result = new ChefResult();
            try
            {

                 result = IsCanAdd(model);
                if (result.IsSuccess == false)
                {
                    return result;
                }

                var stuffprice = _unitOfWork.StuffPriceRepository.SelectQuery()
                     .AsNoTracking()
                     .Where(a => a.Id == model.StuffPriceId).Select(a => new
                     {
                         a.Id,
                         a.Price,
                         a.TotalPrice,
                         a.AmountPercent,
                     }).FirstOrDefault();

                var item = new IngredinsFood()
                {
                    Id = Guid.NewGuid(),
                    StuffPriceId = model.StuffPriceId.Value,
                    FoodProviderId = model.FoodProviderId,
                    Unit = model.Unit,
                    Amount = model.Amount,
                    Cost = model.Amount * stuffprice.TotalPrice

                };
                _unitOfWork.IngredinsFoodRepository.Add(item);
                await _unitOfWork.SaveAsync();
                return result;
            }
            catch (Exception ex)
            {
                result.AddError("عملیات نا موفق بود");
                return result;
            }

        }

        private ChefResult IsCanAdd(CreateIngredinsFoodVM model)
        {
            var result = new ChefResult();
            if (IsDuplicate(model))
            {
                result.AddError("مورد تکراری است");
                return result;
            }
            if (!model.Amount.HasValue)
            {
                result.AddError("لطفا مقدار مورد نیاز را وارد کنید");
                return result;
            }
            if (!model.StuffPriceId.HasValue)
            {
                result.AddError("لطفا مواد غذایی مورد نیاز را وارد کنید");
                return result;
            }
            if (string.IsNullOrEmpty(model.Unit))
            {
                result.AddError("لطفا واحد اندازه گیری را وارد کنید");
                return result;
            }
            return result;
        }

        public async Task<ChefResult> Delete(SearchIngredinsFoodVm model)
        {
            var result = new ChefResult();
            try
            {


                var find = _unitOfWork.IngredinsFoodRepository.Select(a => a.Id == model.Id);
                if (find == null)
                {
                    result.AddError("داده مورد نظر حذف شده است");
                    return result;
                }

                _unitOfWork.IngredinsFoodRepository.Delete(find);
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

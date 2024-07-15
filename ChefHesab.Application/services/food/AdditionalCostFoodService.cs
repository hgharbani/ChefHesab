using AutoMapper;
using ChefHesab.Application.Interface.food;
using ChefHesab.Dto.food.AdditionalCostFood;
using ChefHesab.Dto.food.IngredinsFood;
using ChefHesab.Share.model;
using ChefHesab.Share.model.KendoModel.Response;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using ChefHesab.Share.Extiontions.KendoExtentions;
namespace ChefHesab.Domain.Peresentition.IRepositories
{
    /// <summary>
    /// هزینه های جانبی
    /// </summary>
    public class AdditionalCostFoodService : IAdditionalCostFoodService
    {
        public IChefHesabUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AdditionalCostFoodService(IChefHesabUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        private bool IsDuplicate(CreateAdditionalCostFoodVM model)
        {
            if (!string.IsNullOrEmpty(model.Id.ToString()))
            {
                return _unitOfWork.AdditionalCostFoodRepository
                    .Any(a => a.Id != model.Id
                    && a.FoodProviderId == model.FoodProviderId
                    && a.AdditionalCostId == model.AdditionalCostId);

            }
            else
            {
                return _unitOfWork.AdditionalCostFoodRepository
                    .Any(a => a.FoodProviderId == model.FoodProviderId
                    && a.AdditionalCostId == model.AdditionalCostId);
            }

        }
        public dynamic GetAdditionalCostFood(SearchAdditionalCostfoodVM request)
        {
            var foodproviderQuery = _unitOfWork
                .AdditionalCostFoodRepository
                .SelectQuery().Where(a => a.FoodProviderId == request.FoodProviderId)
                .Include(a => a.AdditionalCost)
                .AsNoTracking();

            var SumIngredinsFood = _unitOfWork
           .IngredinsFoodRepository
           .SelectQuery().Where(a => a.FoodProviderId == request.FoodProviderId).Sum(a => a.Cost.Value);          
        
            int total = foodproviderQuery.Count();
            IList resultData;
            bool isGrouped = false;
            var aggregates = new Dictionary<string, Dictionary<string, string>>();

            var data = foodproviderQuery.Select(a => new AdditionalCostFoodVM()
            {
                Id = a.Id,
                Title = a.AdditionalCost.Title,
                IsShowRatio = a.AdditionalCost.IsShowRatio,
                Price = a.AdditionalCost.Price.Value,
                Cost= a.AdditionalCost.IsShowRatio?0: a.AdditionalCost.Price.Value,
                
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
                var datalist = data.ToList();
                var SumListIsShowRatioIsFalse = datalist.Where(a=>!a.IsShowRatio).Sum(a => a.Price);
                SumIngredinsFood = SumIngredinsFood + SumListIsShowRatioIsFalse;
                foreach (var item in datalist.Where(a=>a.IsShowRatio))
                {
                    if (item.IsShowRatio)
                    {
                        double price = (item.Price*0.01);
                        var CostRatio = SumIngredinsFood * price;
                        item.Cost = Math.Floor(CostRatio);
                        SumIngredinsFood= SumIngredinsFood+CostRatio;
                    }
                   
                }
                resultData = datalist;
            }
         
            var result = new Response(resultData, aggregates, total, isGrouped).ToResult();
            return result;
        }


        public async Task<ChefResult> Create(CreateAdditionalCostFoodVM model)
        {
            var result = new ChefResult();
            try
            {

                result = IsCanAdd(model);
                if (result.IsSuccess == false)
                {
                    return result;
                }

                var AdditionalCost = _unitOfWork.AdditionalCostRepository.SelectQuery()
                     .AsNoTracking()
                     .Where(a => a.Id == model.AdditionalCostId).Select(a => new
                     {
                         a.Id,
                         a.Price,
                         a.IsShowRatio,
                     }).FirstOrDefault();

                var item = new AdditionalCostFood()
                {
                    Id = Guid.NewGuid(),
                    AdditionalCostId = model.AdditionalCostId.Value,
                    FoodProviderId = model.FoodProviderId,
                    Ratio = 1,
                   Cost= AdditionalCost.IsShowRatio?0:Convert.ToDouble(AdditionalCost.Price),

                };
                _unitOfWork.AdditionalCostFoodRepository.Add(item);
                await _unitOfWork.SaveAsync();
                return result;
            }
            catch (Exception ex)
            {
                result.AddError("عملیات نا موفق بود");
                return result;
            }

        }

        private ChefResult IsCanAdd(CreateAdditionalCostFoodVM model)
        {
            var result = new ChefResult();
            if (IsDuplicate(model))
            {
                result.AddError("مورد تکراری است");
                return result;
            }
            if (!model.AdditionalCostId.HasValue)
            {
                result.AddError("لطفا مقدار مورد نیاز را وارد کنید");
                return result;
            }
           
            return result;
        }

        public async Task<ChefResult> Delete(SearchAdditionalCostfoodVM model)
        {
            var result = new ChefResult();
            try
            {


                var find = _unitOfWork.AdditionalCostFoodRepository.Select(a => a.Id == model.Id);
                if (find == null)
                {
                    result.AddError("داده مورد نظر حذف شده است");
                    return result;
                }

                _unitOfWork.AdditionalCostFoodRepository.Delete(find);
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

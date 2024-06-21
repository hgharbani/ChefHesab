using ChefHesab.Application.Interface;
using ChefHesab.Application.Interface.define;
using ChefHesab.Data.Presentition.Context;
using ChefHesab.Data.Presentition.Reositories.generic;
using ChefHesab.Dto.define.FoodCategory;
using ChefHesab.Dto.define.FoodStuff;
using ChefHesab.Share.Extiontions.KendoExtentions;
using ChefHesab.Share.model.KendoModel;
using ChefHesab.Share.model.KendoModel.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ChefHesab.Dto.define.FoodStuff.SnapFoodStufCategory;

namespace ChefHesab.Domain.Peresentition.IRepositories.define
{
    /// <summary>
    /// دسته بندی غذاها
    /// </summary>
    public class FoodCategoryService : IFoodCategoryService
    {
        public IChefHesabUnitOfWork _unitOfWork;

        public FoodCategoryService(IChefHesabUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public dynamic GetAllByKendoFilter(SearchFoodCategoryVM request)
        {
            var data = _unitOfWork.FoodCategoryRepository.GetAllQueryble().Include(_ => _.Parent)
                .AsNoTracking().Where(a => a.Active == true && a.ParentId.HasValue);
                if (request.Code > 0)
            {
                data = data.Where(a => a.Parent.Code == request.Code);
            }
            int total = data.Count();
            IList resultData;
            bool isGrouped = false;

            var aggregates = new Dictionary<string, Dictionary<string, string>>();

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
    }
}

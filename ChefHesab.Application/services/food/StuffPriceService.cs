using AutoMapper;
using ChefHesab.Application.Interface;
using ChefHesab.Application.Interface.food;
using ChefHesab.Data.Presentition.Context;
using ChefHesab.Data.Presentition.Reositories.generic;
using ChefHesab.Dto.define.ContractingCompany;
using ChefHesab.Dto.define.FoodStuff;
using ChefHesab.Dto.food.IngredinsFood;
using ChefHesab.Dto.food.StuffPrice;
using ChefHesab.Share.Extiontions.KendoExtentions;
using ChefHesab.Share.model;
using ChefHesab.Share.model.KendoModel.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Domain.Peresentition.IRepositories.food
{
    /// <summary>
    /// سرویس مدیریت قیمت کالا
    /// </summary>
    public class StuffPriceService : IStuffPriceService
    {
        public IChefHesabUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public StuffPriceService(IChefHesabUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public dynamic GetIngredinsFoods(FoodStuffSearch request)
        {
            var foodproviderQuery = _unitOfWork
                .StuffPriceRepository
                .SelectQuery()
                .Include(a => a.FoodStuff).AsNoTracking();


            foodproviderQuery = foodproviderQuery.Where(a => a.CompanyId == request.CompanyId)
                ;
            int total = foodproviderQuery.Count();
            IList resultData;
            bool isGrouped = false;
            var aggregates = new Dictionary<string, Dictionary<string, string>>();

            var data = foodproviderQuery.Select(a => new StuffPriceVM()
            {
                Id = a.Id,
                FoodStuffTitle = a.FoodStuff.Title,
                Price = a.TotalPrice,
               

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




        public async Task<ChefResult> AddOrUpdate(CreateStuffPriceVM model)
        {
            var result = new ChefResult();
            if (model.Id.HasValue)
            {
                result = await Edit(model);
            }
            else
            {
                result = await Add(model);
            }

            return result;
        }
        private async Task<bool> IsDuplicate(CreateStuffPriceVM model)
        {
            if (model.Id.HasValue)
            {
                return  _unitOfWork.StuffPriceRepository.Any(a => 
                a.Id != model.Id 
                && a.CompanyId == model.CompanyId 
                && a.FoodStuffId == model.FoodStuffId
                );

            }
            else
            {
                return  _unitOfWork.StuffPriceRepository.Any(a =>
                a.CompanyId == model.CompanyId
                && a.FoodStuffId == model.FoodStuffId
                );
            }

        }

        public async Task<ChefResult> Add(CreateStuffPriceVM model)
        {
            var result = new ChefResult();
            try
            {


                var mapper = _mapper.Map<StuffPrice>(model);
                mapper.PersonalId = new Guid("FC769A7E-6A78-42CE-B7F9-0E1619CD5EFB");
                mapper.InsertDate = DateTime.Now;
                mapper.Active = true;
                if (model.AmountPercent > 0)
                {
                    mapper.TotalPrice = (long)((model.Price * model.AmountPercent) + model.Price);
                }
                else
                {
                    mapper.TotalPrice = model.Price;
                }
                _unitOfWork.StuffPriceRepository.Add(mapper);

              await  _unitOfWork.SaveAsync();
                return result;
            }
            catch (Exception ex)
            {
                result.AddError("عملیات نا موفق بود");
                return result;
            }

        }
        public async Task<ChefResult> Edit(CreateStuffPriceVM model)
        {
            var result = new ChefResult();
            try
            {

                if (await IsDuplicate(model))
                {
                    result.AddError("داده وارد شده تکراری می باشد");
                    return result;
                }
                var find = _unitOfWork.StuffPriceRepository.Select(a => a.Id == model.Id);
                if (find == null)
                {
                    result.AddError("داده مورد نظر حذف شده است");
                    return result;
                }
                if (model.AmountPercent > 0)
                {
                    model.TotalPrice = (long)((model.Price * model.AmountPercent) + model.Price);
                }
                else
                {
                    model.TotalPrice = model.Price;
                }
                var ingredins= await _unitOfWork.IngredinsFoodRepository.SelectQuery()
                    .Where(a=>a.StuffPriceId==find.Id).ToListAsync();
                foreach(var item in ingredins)
                {
                    item.Cost = item.Amount * model.TotalPrice;
                    _unitOfWork.IngredinsFoodRepository.Update(item);
                }

                var mapper = _mapper.Map<CreateStuffPriceVM, StuffPrice>(model, find);
                _unitOfWork.StuffPriceRepository.Update(mapper);
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

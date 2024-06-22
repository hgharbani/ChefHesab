using AutoMapper;
using ChefHesab.Application.Interface;
using ChefHesab.Application.Interface.define;
using ChefHesab.Data.Presentition.Context;
using ChefHesab.Data.Presentition.Reositories.generic;
using ChefHesab.Domain;
using ChefHesab.Domain.Peresentition.IRepositories;
using ChefHesab.Domain.Peresentition.IRepositories.define;
using ChefHesab.Domain.Peresentition.IRepositories.IGenericRepository;
using ChefHesab.Dto.define.AdditionalCost;
using ChefHesab.Share.model;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Application.services.define
{
    /// <summary>
    /// تعاریف هزینه های جانبی
    /// </summary>
    public class AdditionalCostService : IAdditionalCostService
    {
        public IChefHesabUnitOfWork _unitOfWork;
        public Mapper _mapper;

        public AdditionalCostService(IChefHesabUnitOfWork unitOfWork, Mapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        private async Task<bool> IsDuplicate(AdditionalCostVM additionalCost)
        {
            if (string.IsNullOrEmpty(additionalCost.Id.ToString()))
            {
                return  _unitOfWork.AdditionalCostRepository.Any(a =>a.Id!=additionalCost.Id&& a.Title == additionalCost.Title);

            }
            else
            {
                return  _unitOfWork.AdditionalCostRepository.Any(a => a.Title == additionalCost.Title);
            }
            
        }
        public List<AdditionalCost> GetAdditionalCost()
        {
            return _unitOfWork.AdditionalCostRepository.SelectAll().ToList();
        }
        public async Task<Tuple<List<AdditionalCostVM>,int>> GetAdditionalCostListWithPeginition(AdditionalCostVM additionalCost)
        {
            var filters = new List<Expression<Func<AdditionalCost, bool>>>();
            if (string.IsNullOrEmpty(additionalCost.Title))
            {
                filters.Add(a => a.Title == "");
            }
            

            var result =await _unitOfWork.AdditionalCostRepository.SelectDataFilteredByPage(additionalCost.pageNumber, additionalCost.pageSize, filters);
            var mappedResult= result.Item2.Select(a=>_mapper.Map<AdditionalCostVM>(a)).ToList();
            return new Tuple<List<AdditionalCostVM>, int>(mappedResult,result.Item1);

        }

        public async Task<ChefResult> Add(AdditionalCostVM additionalCost)
        {
            var result = new ChefResult();
            try
            {
              
                if (await IsDuplicate(additionalCost))
                {
                    result.AddError("داده وارد شده تکراری می باشد");
                    return result;
                }
                var mapper = _mapper.Map<AdditionalCost>(additionalCost);
                _unitOfWork.AdditionalCostRepository.Add(mapper);
                await _unitOfWork.SaveAsync();
                return result;
            }
            catch (Exception ex)
            {
                result.AddError("عملیات نا موفق بود");
                return result;
            }
            
        }
        public async Task<ChefResult> Edit(AdditionalCostVM additionalCost)
        {
            var result = new ChefResult();
            try
            {

                if (await IsDuplicate(additionalCost))
                {
                    result.AddError("داده وارد شده تکراری می باشد");
                    return result;
                }
                var find = _unitOfWork.AdditionalCostRepository.Where(a=>a.Id== additionalCost.Id).FirstOrDefault();
                if (find == null)
                {
                    result.AddError("داده مورد نظر حذف شده است");
                    return result;
                }

                var mapper = _mapper.Map<AdditionalCostVM,AdditionalCost>(additionalCost, find);
                _unitOfWork.AdditionalCostRepository.Update(mapper);
                 _unitOfWork.SaveAsync();
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

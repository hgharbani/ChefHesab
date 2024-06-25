using AutoMapper;
using ChefHesab.Application.Interface.define;
using ChefHesab.Domain;
using ChefHesab.Domain.Peresentition.IRepositories;
using ChefHesab.Dto.define.AdditionalCost;
using ChefHesab.Share.Extiontions.KendoExtentions;
using ChefHesab.Share.model;
using ChefHesab.Share.model.KendoModel.Response;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace ChefHesab.Application.services.define
{
    /// <summary>
    /// تعاریف هزینه های جانبی
    /// </summary>
    public class AdditionalCostService : IAdditionalCostService
    {
        public IChefHesabUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AdditionalCostService(IChefHesabUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        private bool IsDuplicate(CreateAdditionalCostVM additionalCost)
        {
            if (string.IsNullOrEmpty(additionalCost.Id.ToString()))
            {
                return _unitOfWork.AdditionalCostRepository
                    .Any(a => a.Id != additionalCost.Id
                    && a.Title == additionalCost.Title
                    && a.CompanyId == additionalCost.CompanyId
                   && a.IsActive.HasValue && a.IsActive.Value == true);

            }
            else
            {
                return _unitOfWork.AdditionalCostRepository
                    .Any(a => a.Title == additionalCost.Title && a.IsActive.HasValue && a.IsActive.Value==true
                    && a.CompanyId == additionalCost.CompanyId);
            }

        }
        public dynamic GetAdditionalCostWithCompany(AdditionalCostSearchModel request)

        {
            var additionalCosts = _unitOfWork
                .AdditionalCostRepository
                .GetAll()
                .Include(a => a.ContractingCompany).Where(a=>a.IsActive==true).AsTracking();
            if (request.CompanyId.HasValue)
            {

                additionalCosts = additionalCosts.Where(a => a.CompanyId == request.CompanyId.Value);
            }

            int total = additionalCosts.Count();
            IList resultData;
            bool isGrouped = false;
            var aggregates = new Dictionary<string, Dictionary<string, string>>();
            var data = additionalCosts.Select(a => new AdditionalCostVM()
            {
                Id = a.Id,
                Title = a.Title,
                IsShowRatio = a.IsShowRatio,
                Price = a.Price,
                CompanyTitle = a.ContractingCompany.CompanyName,
                CompanyId = a.CompanyId
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
        public CreateAdditionalCostVM GetForEdit(Guid id)
        {
            var result = _unitOfWork.AdditionalCostRepository
                .Where(a => a.Id == id)
                .Select(a => _mapper.Map<AdditionalCost, CreateAdditionalCostVM>(a))
                .FirstOrDefault();
            return result;
        }


        public async Task<ChefResult> Add(CreateAdditionalCostVM additionalCost)
        {
            var result = new ChefResult();
            try
            {

                if (IsDuplicate(additionalCost))
                {
                    result.AddError("داده وارد شده تکراری می باشد");
                    return result;
                }
                var mapper = _mapper.Map<AdditionalCost>(additionalCost);
                mapper.IsActive = true;
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
        public async Task<ChefResult> Edit(CreateAdditionalCostVM additionalCost)
        {
            var result = new ChefResult();
            try
            {

                if (IsDuplicate(additionalCost))
                {
                    result.AddError("داده وارد شده تکراری می باشد");
                    return result;
                }
                var find = _unitOfWork.AdditionalCostRepository.Where(a => a.Id == additionalCost.Id).FirstOrDefault();
                if (find == null)
                {
                    result.AddError("داده مورد نظر حذف شده است");
                    return result;
                }

                var mapper = _mapper.Map<CreateAdditionalCostVM, AdditionalCost>(additionalCost, find);
                _unitOfWork.AdditionalCostRepository.Update(mapper);
                await _unitOfWork.SaveAsync();
                return result;
            }
            catch (Exception ex)
            {
                result.AddError("عملیات نا موفق بود");
                return result;
            }

        }
        public async Task<ChefResult> Delete(CreateAdditionalCostVM additionalCost)
        {
            var result = new ChefResult();
            try
            {
                var find = _unitOfWork.AdditionalCostRepository.Where(a => a.Id == additionalCost.Id).FirstOrDefault();
                if (find == null)
                {
                    result.AddError("داده مورد نظر حذف شده است");
                    return result;
                }


                find.IsActive = false;
                _unitOfWork.AdditionalCostRepository.Update(find);
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

using AutoMapper;
using ChefHesab.Application.Interface;
using ChefHesab.Application.Interface.define;
using ChefHesab.Data.Presentition.Context;
using ChefHesab.Data.Presentition.Reositories.generic;
using ChefHesab.Dto.define.ContractingCompany;
using ChefHesab.Share.model;
using ChefHesab.Share.model.KendoModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ChefHesab.Share.Extiontions.KendoExtentions;
using ChefHesab.Share.model.KendoModel.Response;
using ChefHesab.Domain.Peresentition.IRepositories;
using ChefHesab.Domain;

namespace ChefHesab.Application.services.define
{
    /// <summary>
    /// شرکت های طرف قرار داد
    /// </summary>
    public class ContractingCompanyService : IContractingCompanyService
    {
        public IChefHesabUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ContractingCompanyService(IChefHesabUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        private bool IsDuplicate(ContractingCompanyVM contractingCompany)
        {
            if (!string.IsNullOrEmpty(contractingCompany.Id.ToString()))
            {
                return  _unitOfWork.ContractingCompanyRepository
                    .Any(a => a.Id != contractingCompany.Id && 
                    a.CompanyName == contractingCompany.CompanyName &&
                    ((a.AgreementDate <= contractingCompany.AgreementDate &&
                    a.ExpirationDate >= contractingCompany.AgreementDate)
                ||
                     (a.AgreementDate <= contractingCompany.ExpirationDate 
                        && a.ExpirationDate >= contractingCompany.ExpirationDate))
                   );

            }
            else
            {
                return  _unitOfWork.ContractingCompanyRepository.Any(a =>  a.CompanyName == contractingCompany.CompanyName &&
                ((a.AgreementDate <= contractingCompany.AgreementDate && a.ExpirationDate >= contractingCompany.AgreementDate)
                ||
                (a.AgreementDate <= contractingCompany.ExpirationDate && a.ExpirationDate >= contractingCompany.ExpirationDate))
                );
            }

        }
        public List<ContractingCompanyVM> GetContractingCompany()
        {

            return _unitOfWork.ContractingCompanyRepository.SelectAll().Select(a => _mapper.Map<ContractingCompanyVM>(a)).ToList();
        }

        public ContractingCompanyVM GetOne(Guid contractingId)
        {
            var result = _unitOfWork.ContractingCompanyRepository.Where(a => a.Id == contractingId).Select(a => _mapper.Map<ContractingCompanyVM>(a)).FirstOrDefault();
            return result;
        }
        public dynamic GetAllByKendoFilter(Request request)
        {
            var data = _unitOfWork.ContractingCompanyRepository.GetAllAsNoTracking().Where(a=>a.IsActive.Value==true);
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
        public async Task<Tuple<List<ContractingCompanyVM>, int>> GetContractingCompanyListWithPeginition(ContractingCompanyVM contractingCompany)
        {
            var filters = new List<Expression<Func<ContractingCompany, bool>>>();
            if (string.IsNullOrEmpty(contractingCompany.CompanyName))
            {
                filters.Add(a => a.CompanyName == "");
            }


            // var result = await _unitOfWork.ContractingCompanyRepository.SelectDataFilteredByPage(contractingCompany.pageNumber, contractingCompany.pageSize, filters);
            // var mappedResult = result.Item2.Select(a => _mapper.Map<ContractingCompanyVM>(a)).ToList();
            return new Tuple<List<ContractingCompanyVM>, int>(new List<ContractingCompanyVM>(), 0);

        }

        public async Task<ChefResult> Add(ContractingCompanyVM model)
        {
            var result = new ChefResult();
            try
            {

                if ( IsDuplicate(model))
                {
                    result.AddError("داده وارد شده تکراری می باشد");
                    return result;
                }
                var mapper = _mapper.Map<ContractingCompany>(model);
                mapper.PersonalId = new Guid("FC769A7E-6A78-42CE-B7F9-0E1619CD5EFB");
                mapper.AgreementPeriod = mapper.ExpirationDate.Value.Date.Subtract(mapper.AgreementDate.Value.Date).Days;
                mapper.IsActive = true;
                _unitOfWork.ContractingCompanyRepository.Add(mapper);

                var id = await _unitOfWork.SaveAsync();
                if (id > 0)
                {

                    return result;
                }

                result.AddError("اطلاعاتی تغییر نیافته است");
                return result;

            }
            catch (Exception ex)
            {
                result.AddError("عملیات نا موفق بود");
                return result;
            }

        }
        public async Task<ChefResult> Edit(ContractingCompanyVM ContractingCompany)
        {
            var result = new ChefResult();
            try
            {

                if ( IsDuplicate(ContractingCompany))
                {
                    result.AddError("داده وارد شده تکراری می باشد");
                    return result;
                }
                var find = _unitOfWork.ContractingCompanyRepository.Select(a=>a.Id==ContractingCompany.Id);
                if (find == null)
                {
                    result.AddError("داده مورد نظر حذف شده است");
                    return result;
                }

                var mapper = _mapper.Map<ContractingCompanyVM, ContractingCompany>(ContractingCompany, find);
                mapper.AgreementPeriod = mapper.ExpirationDate.Value.Date.Subtract(mapper.AgreementDate.Value.Date).Days;
                _unitOfWork.ContractingCompanyRepository.Update(mapper);
             var id=await  _unitOfWork.SaveAsync();
                if (id > 0)
                {

                return result;
                }
               
                    result.AddError("اطلاعاتی تغییر نیافته است");
                    return result;
                
            }
            catch (Exception ex)
            {
                result.AddError("عملیات نا موفق بود");
                return result;
            }

        }
        public async Task<ChefResult> Delete(CoontractingCompanySearch model)
        {
            var result = new ChefResult();
            try
            {


                var find = _unitOfWork.ContractingCompanyRepository.Select(a => a.Id == model.Id);
                if (find == null)
                {
                    result.AddError("داده مورد نظر حذف شده است");
                    return result;
                }


                find.IsActive = false;
                _unitOfWork.ContractingCompanyRepository.Update(find);
                var id = await _unitOfWork.SaveAsync();
                if (id > 0)
                {

                    return result;
                }

                result.AddError("اطلاعاتی تغییر نیافته است");
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

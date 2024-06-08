using AutoMapper;
using ChefHesab.Application.Interface;
using ChefHesab.Application.Interface.define;
using ChefHesab.Data.Presentition.Context;
using ChefHesab.Data.Presentition.Reositories.generic;
using ChefHesab.Dto.define.ContractingCompany;
using ChefHesab.Share.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Domain.Peresentition.IRepositories.define
{
    /// <summary>
    /// شرکت های طرف قرار داد
    /// </summary>
    public class ContractingCompanyService : IContractingCompanyService
    {
        public IChefHesabUnitOfWork _unitOfWork;
        public Mapper _mapper;
        public ContractingCompanyService(IChefHesabUnitOfWork unitOfWork, Mapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        private async Task<bool> IsDuplicate(ContractingCompanyVM contractingCompany)
        {
            if (string.IsNullOrEmpty(contractingCompany.Id.ToString()))
            {
                return await _unitOfWork.ContractingCompanyRepository.Any(a => a.Id != contractingCompany.Id && a.CompanyName == contractingCompany.CompanyName);

            }
            else
            {
                return await _unitOfWork.ContractingCompanyRepository.Any(a => a.CompanyName == contractingCompany.CompanyName);
            }

        }
        public List<ContractingCompany> GetContractingCompany()
        {
            return _unitOfWork.ContractingCompanyRepository.SelectAll().ToList();
        }
        public async Task<Tuple<List<ContractingCompanyVM>, int>> GetContractingCompanyListWithPeginition(ContractingCompanyVM contractingCompany)
        {
            var filters = new List<Expression<Func<ContractingCompany, bool>>>();
            if (string.IsNullOrEmpty(contractingCompany.CompanyName))
            {
                filters.Add(a => a.CompanyName == "");
            }


            var result = await _unitOfWork.ContractingCompanyRepository.SelectDataFilteredByPage(contractingCompany.pageNumber, contractingCompany.pageSize, filters);
            var mappedResult = result.Item2.Select(a => _mapper.Map<ContractingCompanyVM>(a)).ToList();
            return new Tuple<List<ContractingCompanyVM>, int>(mappedResult, result.Item1);

        }

        public async Task<ChefResult> Add(ContractingCompanyVM ContractingCompany)
        {
            var result = new ChefResult();
            try
            {

                if (await IsDuplicate(ContractingCompany))
                {
                    result.AddError("داده وارد شده تکراری می باشد");
                    return result;
                }
                var mapper = _mapper.Map<ContractingCompany>(ContractingCompany);
                _unitOfWork.ContractingCompanyRepository.Insert(mapper);
                await _unitOfWork.SaveAsync();
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

                if (await IsDuplicate(ContractingCompany))
                {
                    result.AddError("داده وارد شده تکراری می باشد");
                    return result;
                }
                var find = _unitOfWork.ContractingCompanyRepository.SelectByKey(ContractingCompany.Id);
                if (find == null)
                {
                    result.AddError("داده مورد نظر حذف شده است");
                    return result;
                }

                var mapper = _mapper.Map<ContractingCompanyVM, ContractingCompany>(ContractingCompany, find);
                _unitOfWork.ContractingCompanyRepository.Update(mapper);
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

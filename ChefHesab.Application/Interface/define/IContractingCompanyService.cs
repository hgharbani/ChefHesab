using ChefHesab.Domain;
using ChefHesab.Dto.define.ContractingCompany;
using ChefHesab.Share.model;
using ChefHesab.Share.model.KendoModel;

namespace ChefHesab.Application.Interface.define
{
    public interface IContractingCompanyService
    {
        Task<ChefResult> Add(ContractingCompanyVM ContractingCompany);
        Task<ChefResult> Edit(ContractingCompanyVM ContractingCompany);
        dynamic GetAllByKendoFilter(Request request);
        List<ContractingCompanyVM> GetContractingCompany();
        Task<Tuple<List<ContractingCompanyVM>, int>> GetContractingCompanyListWithPeginition(ContractingCompanyVM contractingCompany);
    }
}
using ChefHesab.Domain;
using ChefHesab.Dto.define.ContractingCompany;
using ChefHesab.Share.model;

namespace ChefHesab.Application.Interface.define
{
    public interface IContractingCompanyService
    {
        Task<ChefResult> Add(ContractingCompanyVM ContractingCompany);
        Task<ChefResult> Edit(ContractingCompanyVM ContractingCompany);
        List<ContractingCompany> GetContractingCompany();
        Task<Tuple<List<ContractingCompanyVM>, int>> GetContractingCompanyListWithPeginition(ContractingCompanyVM contractingCompany);
    }
}
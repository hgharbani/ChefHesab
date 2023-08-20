using AutoMapper;
using System;
using ChefHesab.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChefHesab.Dto.define.FoodCategory;
using ChefHesab.Dto.define.ContractingCompany;

namespace ChefHesab.Application.AutoMapper
{
    public class ContractingCompanyProfile : Profile
    {
        public ContractingCompanyProfile()
        {
            CreateMap<ContractingCompany, ContractingCompanyVM>().ReverseMap();
            CreateMap<ContractingCompany, ContractingCompanyVM>().ReverseMap();
        }
    }
}

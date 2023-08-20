using AutoMapper;
using ChefHesab.Domain;
using ChefHesab.Dto.define.AdditionalCost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Application.AutoMapper
{
    public class AdditionalCostProfile : Profile
    {
        public AdditionalCostProfile()
        {
                  CreateMap<AdditionalCost, AdditionalCostVM>().ReverseMap();
                  CreateMap<AdditionalCost, CreateAdditionalCostVM>().ReverseMap();
        }
    }
}

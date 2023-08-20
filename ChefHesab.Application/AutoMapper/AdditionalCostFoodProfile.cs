using AutoMapper;
using ChefHesab.Domain;
using ChefHesab.Dto.food.AdditionalCostFood;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Application.AutoMapper
{
    public class AdditionalCostFoodProfile : Profile
    {
        public AdditionalCostFoodProfile()
        {
            CreateMap<AdditionalCostFood, AdditionalCostFoodVM>().ReverseMap();
            CreateMap<AdditionalCostFood, CreateAdditionalCostFoodVM>().ReverseMap();
        }
    }
}

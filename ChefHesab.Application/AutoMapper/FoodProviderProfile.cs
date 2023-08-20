using AutoMapper;
using ChefHesab.Domain;
using ChefHesab.Dto.define.FoodStuff;
using ChefHesab.Dto.food.FoodProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Application.AutoMapper
{
    public class FoodProviderProfile : Profile
    {
        public FoodProviderProfile()
        {
            CreateMap<FoodProvider, FoodProviderVM>().ReverseMap();
            CreateMap<FoodProvider, CreateFoodProviderVM>().ReverseMap();
        }
    }
}

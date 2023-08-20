using AutoMapper;
using ChefHesab.Domain;
using ChefHesab.Dto.define.FoodStuff;
using ChefHesab.Dto.food.IngredinsFood;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Application.AutoMapper
{
    public class FoodStuffProfile : Profile
    {
        public FoodStuffProfile()
        {
            CreateMap<FoodStuff, FoodStuffVM>().ReverseMap();
            CreateMap<FoodStuff, CreateFoodStuffVM>().ReverseMap();
        }
    }
}

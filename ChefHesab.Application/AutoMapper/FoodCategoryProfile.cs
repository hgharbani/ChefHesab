using AutoMapper;
using ChefHesab.Domain;
using ChefHesab.Dto.define.FoodCategory;
using ChefHesab.Dto.food.FoodProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Application.AutoMapper
{
    public class FoodCategoryProfile : Profile
    {
        public FoodCategoryProfile()
        {
            CreateMap<FoodCategory, FoodCategoryVM>().ReverseMap();
            CreateMap<FoodCategory, CreateFoodCategoryVM>().ReverseMap();
        }
    }
}

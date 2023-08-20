using AutoMapper;
using System;
using ChefHesab.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChefHesab.Dto.food.StuffPrice;
using ChefHesab.Dto.food.IngredinsFood;

namespace ChefHesab.Application.AutoMapper
{
    public class IngredinsFoodProfile : Profile
    {
        public IngredinsFoodProfile()
        {
            CreateMap<IngredinsFood, IngredinsFoodVM>().ReverseMap();
            CreateMap<IngredinsFood, CreateIngredinsFoodVM>().ReverseMap();
        }
    }
}

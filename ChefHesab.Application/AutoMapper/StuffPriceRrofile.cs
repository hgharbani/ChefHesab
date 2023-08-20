using AutoMapper;
using System;
using ChefHesab.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChefHesab.Dto.food.StuffPrice;

namespace ChefHesab.Application.AutoMapper
{
    public class StuffPriceRrofile : Profile
    {
        public StuffPriceRrofile()
        {
            CreateMap<StuffPrice, StuffPriceVM>().ReverseMap();  
            CreateMap<StuffPrice, CreateStuffPriceVM>().ReverseMap();
        }
    }
}

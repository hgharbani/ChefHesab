using AutoMapper;
using ChefHesab.Domain;
using ChefHesab.Dto.define.Personal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Application.AutoMapper
{
    public class PersonalProfile : Profile
    {
        public PersonalProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<Personal, PersonalVM>();
            CreateMap<PersonalVM, Personal>();
        }
    }
}

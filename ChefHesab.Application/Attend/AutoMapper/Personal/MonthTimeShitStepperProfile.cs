using AutoMapper;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.DTO.Personal.MonthTimeShitStepper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Profiles.Transfer
{
    public class MonthTimeShitStepperProfile : Profile
    {
        public MonthTimeShitStepperProfile()
        {
            CreateMap<SearchMonthTimeShitStepperModel, MonthTimeShitStepper>().ReverseMap();

            CreateMap<MonthTimeShitStepper, MonthTimeShitStepperViewModel>().ReverseMap() ;
            CreateMap<MonthTimeShitStepper, AddOrEditMonthTimeShitStepperModel>().ReverseMap().AfterMap((src, dest) =>
            {
             
                dest.IsActive = true;
            }).ReverseMap();
           
        }
    }
}

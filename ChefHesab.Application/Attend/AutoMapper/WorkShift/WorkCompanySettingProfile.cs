using AutoMapper;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.WorkCompanySetting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Profiles.WorkShift
{
    public class WorkCompanySettingProfile : Profile
    {
        public WorkCompanySettingProfile()
        {
            CreateMap<WorkCompanySettingModel, WorkCompanySetting>().ReverseMap();
            CreateMap<AddWorkCompanySettingModel, WorkCompanySetting>().AfterMap((src, dest) =>
            {
                dest.InsertDate = DateTime.Now;
                dest.InsertUser = src.CurrentUserName;
                dest.DomainName = src.DomainName;
                dest.IsActive = true;
               // dest.CompnayId = 1;
            }).ReverseMap();
            CreateMap<EditWorkCompanySettingModel, WorkCompanySetting>().AfterMap((src, dest) =>
            {
                dest.UpdateDate = DateTime.Now;
                dest.UpdateUser = src.CurrentUserName;             
                
            }).ReverseMap(); 
            CreateMap<SearchWorkCompanySettingModel, WorkCompanySetting>().ReverseMap();
        }
    }
}


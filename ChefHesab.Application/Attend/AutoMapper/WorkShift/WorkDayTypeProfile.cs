using AutoMapper;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.WorkDayType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Profiles.WorkShift
{
    public class WorkDayTypeProfile: Profile
    {
        public WorkDayTypeProfile()
        {
            CreateMap<WorkDayTypeModel, WorkDayType>().ReverseMap();
            CreateMap<AddWorkDayTypeModel, WorkDayType>().ReverseMap();
            CreateMap<EditWorkDayTypeModel, WorkDayType>().ReverseMap();
            CreateMap<SearchWorkDayTypeModel, WorkDayType>().ReverseMap();
        }
    }
}

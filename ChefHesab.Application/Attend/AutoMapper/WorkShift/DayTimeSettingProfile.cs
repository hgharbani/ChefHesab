using AutoMapper;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.DayTimeSetting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Profiles.WorkShift
{
    public class DayTimeSettingProfile : Profile
    {
        public DayTimeSettingProfile()
        {
            CreateMap<SearchDayTimeSettingModel, DayTimeSetting>().ReverseMap();

        }
    }
}

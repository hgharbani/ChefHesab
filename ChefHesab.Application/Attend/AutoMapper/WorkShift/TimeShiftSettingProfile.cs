using AutoMapper;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.TimeShiftSetting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Profiles.WorkShift
{
    public class TimeShiftSettingProfile:Profile
    {
        /// <summary>
        ///
        /// </summary>
        public TimeShiftSettingProfile()
        {
            CreateMap<TimeShiftSettingModel, TimeShiftSetting>().ReverseMap();
            CreateMap<AddTimeShiftSettingModel, TimeShiftSetting>().ReverseMap();
            CreateMap<EditTimeShiftSettingModel, TimeShiftSetting>().ReverseMap();
            CreateMap<SearchTimeShiftSettingModel, TimeShiftSetting>().ReverseMap();
        }
    }
}

using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.RollCallWorkTimeMonthSetting;
using AutoMapper;

namespace Ksc.HR.Appication.Profiles.WorkShift
{
    public class RollCallWorkTimeMonthSettingProfile : Profile
    {
        /// <summary>
        ///
        /// </summary>
        public RollCallWorkTimeMonthSettingProfile()
        {
            CreateMap<RollCallWorkTimeMonthSetting, SearchRollCallWorkTimeMonthSettingModel>().ReverseMap(); ;

        }
    }
}

using AutoMapper;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.ShiftHoliday;


namespace Ksc.HR.Appication.Profiles.WorkShift
{
    public class ShiftHolidayProfile:Profile
    {
        /// <summary>
        ///
        /// </summary>
        public ShiftHolidayProfile()
        {
            CreateMap<ShiftHolidayModel, ShiftHoliday>().ReverseMap();
            CreateMap<AddShiftHolidayModel, ShiftHoliday>().ReverseMap();
            CreateMap<EditShiftHolidayModel, ShiftHoliday>().ReverseMap();
            CreateMap<SearchShiftHolidayModel, ShiftHoliday>().ReverseMap();
        }
    }
}

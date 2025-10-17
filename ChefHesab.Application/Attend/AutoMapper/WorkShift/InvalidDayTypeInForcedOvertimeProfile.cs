using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.InvalidDayTypeInForcedOvertime;
using AutoMapper;

namespace Ksc.HR.Appication.Profiles.WorkShift
{
    public class InvalidDayTypeInForcedOvertimeProfile : Profile
    {
        /// <summary>
        ///
        /// </summary>
        public InvalidDayTypeInForcedOvertimeProfile()
        {
            CreateMap<InvalidDayTypeInForcedOvertime, InvalidDayTypeInForcedOvertimeModel>().ReverseMap(); ;
            CreateMap<InvalidDayTypeInForcedOvertime, AddInvalidDayTypeInForcedOvertimeModel>().ReverseMap().AfterMap((src, dest) =>
            {               
                    dest.InsertDate = System.DateTime.Now;
                dest.InsertUser = src.CurrentUserName;
                dest.IsActive = true;             
            }).ReverseMap();
            CreateMap<InvalidDayTypeInForcedOvertime, EditInvalidDayTypeInForcedOvertimeModel>().ReverseMap().AfterMap((src, dest) =>
            {
                dest.UpdateUser = src.CurrentUserName;
                dest.UpdateDate = System.DateTime.Now;

            }).ReverseMap();

        }
    }
}

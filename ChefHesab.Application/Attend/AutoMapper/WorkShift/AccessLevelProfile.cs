using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.AccessLevel;
using AutoMapper;

namespace Ksc.HR.Appication.Profiles.WorkShift
{
    public class AccessLevelProfile : Profile
    {
        /// <summary>
        ///
        /// </summary>
        public AccessLevelProfile()
        {
            CreateMap<AccessLevel, AccessLevelModel>().ReverseMap(); ;
            CreateMap<AccessLevel, AddAccessLevelModel>().ReverseMap().AfterMap((src, dest) =>
            {               
                    dest.InsertDate = System.DateTime.Now;
                dest.InsertUser = src.CurrentUserName;
                dest.IsActive = true;             
            }).ReverseMap();
            CreateMap<AccessLevel, EditAccessLevelModel>().ReverseMap().AfterMap((src, dest) =>
            {
                dest.UpdateUser = src.CurrentUserName;
                dest.UpdateDate = System.DateTime.Now;
               
            }).ReverseMap();

        }
    }
}

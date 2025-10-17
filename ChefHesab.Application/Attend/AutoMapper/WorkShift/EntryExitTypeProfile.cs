using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.EntryExitType;
using AutoMapper;

namespace Ksc.HR.Appication.Profiles.WorkShift
{
    public class EntryExitTypeProfile : Profile
    {
        /// <summary>
        ///
        /// </summary>
        public EntryExitTypeProfile()
        {
            CreateMap<EntryExitTypeModel, EntryExitType>();
            CreateMap<EntryExitType, EntryExitTypeModel>();
            CreateMap<AddOrEditEntryExitTypeModel, EntryExitType>();
            CreateMap<EntryExitType, AddOrEditEntryExitTypeModel>();
        }
    }
}

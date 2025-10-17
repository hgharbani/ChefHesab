using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.RollCallCategory;
using AutoMapper;

namespace Ksc.HR.Appication.Profiles.WorkShift
{
    public class RollCallCategoryProfile : Profile
    {
        /// <summary>
        ///
        /// </summary>
        public RollCallCategoryProfile()
        {
            CreateMap<RollCallCategoryModel, RollCallCategory>();
            CreateMap<RollCallCategory, RollCallCategoryModel>();
            CreateMap<AddRollCallCategoryModel, RollCallCategory>();
            CreateMap<RollCallCategory, AddRollCallCategoryModel>(); 
            CreateMap<EditRollCallCategoryModel, RollCallCategory>();
            CreateMap<RollCallCategory, EditRollCallCategoryModel>();
        }
    }
}

using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.WorkTimeCategory;
using AutoMapper;

namespace Ksc.HR.Appication.Profiles.WorkShift
{
    public class WorkTimeCategoryProfile : Profile
    {
        /// <summary>
        ///
        /// </summary>
        public WorkTimeCategoryProfile()
        {
            CreateMap<WorkTimeCategory, WorkTimeCategoryModel>().ReverseMap();
            CreateMap<WorkTimeCategory, AddOrEditWorkTimeCategoryModel>().ReverseMap().AfterMap((src, dest) =>
            {
                if (src.Id > 0)
                {
                    dest.UpdateDate = System.DateTime.Now;
                    dest.UpdateUser = src.CurrentUserName;
                }
                else
                {
                    dest.InsertDate = System.DateTime.Now;
                    dest.InsertUser = src.CurrentUserName;
                    dest.IsActive = true;
                }
            }).ReverseMap();

          
        }
    }
}

using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.ShiftConcept;
using AutoMapper;

namespace Ksc.HR.Appication.Profiles.WorkShift
{
    public class ShiftConceptProfile : Profile
    {
        /// <summary>
        ///
        /// </summary>
        public ShiftConceptProfile()
        {
            //CreateMap<ShiftConceptModel, ShiftConcept>();
            //CreateMap<ShiftConcept, ShiftConceptModel>();
            //CreateMap<AddOrEditShiftConceptModel, ShiftConcept>();
            CreateMap<ShiftConcept, AddOrEditShiftConceptModel>();

            CreateMap<ShiftConcept, ShiftConceptModel>().ReverseMap();
            CreateMap<ShiftConcept, AddOrEditShiftConceptModel>().ReverseMap().AfterMap((src, dest) =>
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

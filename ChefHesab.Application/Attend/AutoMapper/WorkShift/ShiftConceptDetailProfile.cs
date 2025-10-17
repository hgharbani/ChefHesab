using AutoMapper;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.ShiftConcept;
using Ksc.HR.DTO.WorkShift.ShiftConceptDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Profiles.WorkShift
{
    public class ShiftConceptDetailProfile : Profile
    {
        /// <summary>
        ///
        /// </summary>
        public ShiftConceptDetailProfile()
        {
            //CreateMap<ShiftConceptDetailModel, ShiftConceptDetail>().ReverseMap();    
            //CreateMap<AddShiftConceptDetailModel, ShiftConceptDetail>().ReverseMap();           
            //CreateMap<EditShiftConceptDetailModel, ShiftConceptDetail>().ReverseMap();
            CreateMap<SearchShiftConceptDetailModel, ShiftConceptDetail>().ReverseMap();
            CreateMap<ShiftConceptDetail, ShiftConceptDetailModel>().ReverseMap();
            CreateMap<ShiftConceptDetail, SearchShiftConceptModel>().ReverseMap();

            CreateMap<ShiftConceptDetail, AddShiftConceptDetailModel>().ReverseMap().AfterMap((src, dest) =>
            {
                dest.InsertDate = System.DateTime.Now;
                dest.InsertUser = src.CurrentUserName;
                dest.IsActive = true;
            }).ReverseMap();
            CreateMap<ShiftConceptDetail,EditShiftConceptDetailModel>().ReverseMap().AfterMap((src, dest) =>
            {
                dest.UpdateUser = src.CurrentUserName;
                dest.UpdateDate = System.DateTime.Now;

            }).ReverseMap();
        }
    }
}

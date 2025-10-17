using AutoMapper;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.UnofficialHolidayReason;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Profiles.WorkShift
{
    public class UnofficialHolidayReasonProfile : Profile
    {
        /// <summary>
        ///
        /// </summary>
        public UnofficialHolidayReasonProfile()
        {
            CreateMap<SearchUnofficialHolidayReasonModel, UnofficialHolidayReason>().ReverseMap();

            //CreateMap<UnofficialHolidayReason, UnofficialHolidayReasonModel>().ReverseMap(); ;
            //CreateMap<UnofficialHolidayReason, AddUnofficialHolidayReasonModel>().ReverseMap().AfterMap((src, dest) =>
            //{
            //    dest.InsertDate = System.DateTime.Now;
            //    dest.InsertUser = src.CurrentUserName;
            //    dest.IsActive = true;
            //}).ReverseMap();
            //CreateMap<UnofficialHolidayReason,EditUnofficialHolidayReasonModel>().ReverseMap().AfterMap((src, dest) =>
            //{
            //    dest.UpdateUser = src.CurrentUserName;
            //    dest.UpdateDate = System.DateTime.Now;

            //}).ReverseMap();
        }
    }
}

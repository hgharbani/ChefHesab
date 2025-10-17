using AutoMapper;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.TimeSheetSetting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Profiles.WorkShift
{
    public class TimeSheetSettingProfile : Profile
    {
        /// <summary>
        ///
        /// </summary>
        public TimeSheetSettingProfile()
        {
            //CreateMap<EditTimeSheetSettingModel, TimeSheetSetting>().AfterMap((src, dest) =>
            //{
            //    //dest.UpdateUser = src.UpdateUser;
            //    dest.UpdateDate = System.DateTime.Now;
            //    dest.IsActive = true;
            //}).ReverseMap();
            CreateMap<TimeSheetSetting, EditTimeSheetSettingModel>().ReverseMap()
                .ForMember(x => x.InsertDate, map => map.Ignore())
                .ForMember(x => x.InsertUser, map => map.Ignore())
                .AfterMap((src, dest) =>
                 {
                     if (src.Id > 0)
                     {
                         dest.UpdateDate = System.DateTime.Now;
                         dest.UpdateUser = src.UpdateUser;
                     }
                     //else
                     //{
                     //    dest.InsertDate = System.DateTime.Now;
                     //    dest.InsertUser = src.up;
                     //    dest.IsActive = true;
                     //}
                 }).ReverseMap();
        }
    }
}

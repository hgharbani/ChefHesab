using AutoMapper;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.WorkCompanyUnOfficialHolidaySetting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Profiles.WorkShift
{
    public class WorkCompanyUnOfficialHolidaySettingProfile : Profile
    {
        /// <summary>
        ///
        /// </summary>
        public WorkCompanyUnOfficialHolidaySettingProfile()
        {
            //CreateMap<SearchWorkCompanyUnOfficialHolidaySettingModel, WorkCompanyUnOfficialHolidaySetting>().ReverseMap();

            CreateMap<WorkCompanyUnOfficialHolidaySetting, WorkCompanyUnOfficialHolidaySettingModel>().AfterMap((src, dest) =>
            {
                dest.UnofficialHolidayReasonTitle = src.UnofficialHolidayReason.Title;
                dest.WorkCalendarDateShamsi = src.WorkCalendar.ShamsiDateV1;
                dest.WorkCompanyUnOfficialHolidayJobCategories = string.Join(',', src.WorkCompanyUnOfficialHolidayJobCategories.Select(x => x.CodeCategoryJobCategory));
            }).ReverseMap(); ;
            CreateMap<WorkCompanyUnOfficialHolidaySetting, AddWorkCompanyUnOfficialHolidaySettingModel>().ReverseMap().AfterMap((src, dest) =>
            {
                dest.InsertDate = System.DateTime.Now;
                dest.InsertUser = src.CurrentUserName;
                dest.IsActive = true;
                
            }).ReverseMap();
            CreateMap<WorkCompanyUnOfficialHolidaySetting, EditWorkCompanyUnOfficialHolidaySettingModel>().ReverseMap().AfterMap((src, dest) =>
             {
                 dest.UpdateUser = src.CurrentUserName;
                 dest.UpdateDate = System.DateTime.Now;
                 

             }).ReverseMap();
        }
    }
}

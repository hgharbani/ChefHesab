using AutoMapper;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItem;
using Ksc.HR.Share.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Profiles.Personal
{
    public class EmployeeAttendAbsenceItemProfile : Profile
    {
        /// <summary>
        ///
        /// </summary>
        public EmployeeAttendAbsenceItemProfile()
        {

            CreateMap<EmployeeAttendAbsenceItem, AddEmployeeAttendAbsenceItemModel>().ReverseMap().AfterMap((src, dest) =>
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
                }
            });
            CreateMap<EmployeeAttendAbsenceAnalysisModel, AnalysisEmployeeAttendAbsenceItem>().ReverseMap();

            CreateMap<ReportEmployeeAttendAbsenceItemModel, ReportEmployeeAttendAbsenceItemModel>().ReverseMap();
            //CreateMap<ReportEmployeeAttendAbsenceItemModel, EmployeeAttendAbsenceItem>().ReverseMap().AfterMap((src, dest) =>
            //{
            //    dest.ShamsiDate = src.WorkCalendar.ShamsiDateV1;
            //    dest.WeekDay = src.WorkCalendar.DayNameShamsi;
            //    dest.ShiftConceptDetailCode = src.ShiftConceptDetail_ShiftConceptDetailId.Code;
            //    dest.RollCallDefinitionCode = src.RollCallDefinition.Code;
            //    dest.RollCallDefinitionTitle = src.RollCallDefinition.Title;

            //});
            CreateMap<AddEmployeeAttendAbsenceItemModel, EmployeeAttendAbsenceItemForcedOverTimeModel>().AfterMap((src, dest) =>
            {

                dest.TimeDurationInMinute = src.TimeDuration.ConvertDurationToMinute().Value;
            });

        }
    }
}

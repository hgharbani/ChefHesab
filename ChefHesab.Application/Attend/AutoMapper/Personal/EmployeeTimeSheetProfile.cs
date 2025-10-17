using AutoMapper;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.DTO.Personal.EmployeeTimeSheet;
using Ksc.HR.Share.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Profiles.Personal
{
    public class EmployeeTimeSheetProfile : Profile
    {
        /// <summary>
        ///
        /// </summary>
        public EmployeeTimeSheetProfile()
        {


            CreateMap<EmployeeTimeSheetMonthModel, EmployeeTimeSheet>().AfterMap((src, dest) =>
            {

                dest.CeilingOvertime = (int)src.CeilingOvertime;
                dest.ExcessOverTime = (int)src.ExcessOverTime;
                dest.ExcessOverTimeDuration = Utility.ConvertMinuteToDuration((int)src.ExcessOverTime);
            });

            CreateMap<EmployeeTimeSheetMonthReportModel, EmployeeTimeSheet>().ReverseMap().AfterMap((src, dest) =>
            {
                dest.TeamCode = src.Employee.TeamWork.Code;
                dest.TeamTitle = src.Employee.TeamWork.Title;
                dest.Name = src.Employee.Name;
                dest.Family = src.Employee.Family;

                dest.CeilingOvertime = (int)src.CeilingOvertime;
                dest.ExcessOverTime = (int)src.ExcessOverTime;
                dest.ExcessOverTimeDuration = Utility.ConvertMinuteToDuration((int)src.ExcessOverTime);
            });


        }
    }
}

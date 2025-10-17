using AutoMapper;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.DTO.Personal.EmployeeLongTermAbsences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Profiles.Personal
{
    public class EmployeeLongTermAbsencesProfile : Profile
    {
        /// <summary>
        ///
        /// </summary>
        public EmployeeLongTermAbsencesProfile()
        {
            CreateMap<EmployeeLongTermAbsence, SearchEmployeeLongTermAbsencesModel>().ReverseMap().AfterMap((src, dest) =>
            {
                src.AbsenceDayCount = dest.AbsenceDayCount;
                src.RollCallDefinitionTitle = dest.RollCallDefinition.Title;

            }); ;

            CreateMap<EmployeeLongTermAbsence, AddEmployeeLongTermAbsencesModel>().ReverseMap().AfterMap((src, dest) =>
            {
                dest.InsertDate = System.DateTime.Now;
                dest.InsertUser = src.CurrentUserName;
                dest.IsDeleted = false;

            }); ;
            CreateMap<EmployeeLongTermAbsence, EditEmployeeLongTermAbsencesModel>().ReverseMap().AfterMap((src, dest) =>
            {
                dest.UpdateDate = System.DateTime.Now;
                dest.UpdateUser = src.CurrentUserName;
                dest.IsDeleted = src.IsDeleted;
                src.InsertDate = dest.InsertDate;
                src.InsertUser = dest.InsertUser;

            }); ;
            CreateMap<EmployeeLongTermAbsence, EmployeeLongTermAbsencesModel>().ReverseMap().AfterMap((src, dest) =>
            {
                src.AbsenceDayCount = dest.AbsenceDayCount; 
                src.RollCallDefinitionTitle= dest.RollCallDefinition.Title;
                src.EmployeeNumber = dest.Employee.EmployeeNumber;
                src.FullName = dest.Employee.Name+" "+dest.Employee.Family;


            }); 
        }
    }
}

using AutoMapper;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.DTO.Personal.EmployeeEducationTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Profiles.Personal
{
    public class EmployeeEducationTimeProfile : Profile
    {
        /// <summary>
        ///
        /// </summary>
        public EmployeeEducationTimeProfile()
        {
            
            CreateMap<EmployeeEducationTime, AddEmployeeEducationTimeModel>().ReverseMap().AfterMap((src, dest) =>
            {
                dest.CreateDateTime = System.DateTime.Now;
                dest.CreateUser = src.CurrentUserName;
            });
            
        }
    }
}

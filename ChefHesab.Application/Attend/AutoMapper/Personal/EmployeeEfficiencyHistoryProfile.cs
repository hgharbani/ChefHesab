using AutoMapper;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.DTO.Personal.EmployeeEfficiencyHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Profiles.Personal
{
    public class EmployeeEfficiencyHistoryProfile : Profile
    {
        /// <summary>
        ///
        /// </summary>
        public EmployeeEfficiencyHistoryProfile()
        {


            CreateMap<EmployeeEfficiencyGridManageModel, EmployeeEfficiencyHistory>().ReverseMap();
            CreateMap<SearchEmployeeEfficiencyHistoryModel, EmployeeEfficiencyHistory>().ReverseMap();

            CreateMap<EmployeeEfficiencyHistory, EmployeeEfficiencyHistoryModel>().ReverseMap();
            CreateMap<EmployeeEfficiencyHistory, AddEmployeeEfficiencyHistoryModel>().ReverseMap().AfterMap((src, dest) =>
            {
                dest.InsertDate = System.DateTime.Now;
                dest.InsertUser = src.CurrentUserName;
            });

        }
    }
}

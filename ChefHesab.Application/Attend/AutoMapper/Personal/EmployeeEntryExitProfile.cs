using Ksc.HR.DTO.Personal.EmployeeEntryExit;
using AutoMapper;
using Ksc.HR.Domain.Entities.Oncall;
using Ksc.HR.Domain.Entities.Personal;

namespace Ksc.HR.Appication.Profiles.OnCall
{
    public class EmployeeEntryExitProfile : Profile
    {
        /// <summary>
        ///
        /// </summary>
        public EmployeeEntryExitProfile()
        {
            CreateMap<EmployeeEntryExit, SearchEmployeeEntryExitModel>();
            CreateMap<EditEmployeeEntryExitModel, EmployeeEntryExit>().ReverseMap();
            CreateMap<EmployeeEntryExitModel, EmployeeEntryExit>()
                .AfterMap((model,entity)=>model.FullName= entity.Employee.Name+" "+entity.Employee.Family)
                .ReverseMap();
        }
    }
}

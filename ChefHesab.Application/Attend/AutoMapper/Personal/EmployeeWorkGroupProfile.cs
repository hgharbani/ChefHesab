using AutoMapper;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.DTO.Personal.EmployeeWorkGroups;

namespace Ksc.HR.Appication.Profiles.Personal
{
    public class EmployeeWorkGroupProfile : Profile
    {
        /// <summary>
        ///
        /// </summary>
        public EmployeeWorkGroupProfile()
        {
            CreateMap<EmployeeWorkGroupModel, EmployeeWorkGroup>().ReverseMap();
        }
    }
}

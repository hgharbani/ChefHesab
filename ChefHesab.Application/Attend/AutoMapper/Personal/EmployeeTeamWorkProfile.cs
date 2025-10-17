using Ksc.HR.DTO.Personal.EmployeeTeamWork;
using AutoMapper;
using Ksc.HR.Domain.Entities.Personal;

namespace Ksc.HR.Appication.Profiles.Personal
{
    public class EmployeeTeamWorkProfile : Profile
    {
        /// <summary>
        ///
        /// </summary>
        public EmployeeTeamWorkProfile()
        {
            //CreateMap<EmployeeTeamWork, SearchEmployeeTeamWorkModel>();
            //CreateMap<EditEmployeeTeamWorkModel, EmployeeTeamWork>().ReverseMap();

            CreateMap<SearchEmployeeTeamWorkModel, EmployeeTeamWork>().ReverseMap();

            CreateMap<EmployeeTeamWork, EmployeeTeamWorkModel>().ReverseMap(); ;
            CreateMap<EmployeeTeamWork, AddEmployeeTeamWorkModel>().ReverseMap().AfterMap((src, dest) =>
            {
                //dest.InsertDate = System.DateTime.Now;
                //dest.InsertUser = src.CurrentUserName;
                //dest.IsActive = true;
            }).ReverseMap();
            CreateMap<EmployeeTeamWork, EditEmployeeTeamWorkModel>().ReverseMap().AfterMap((src, dest) =>
            {
                //dest.UpdateUser = src.CurrentUserName;
                //dest.UpdateDate = System.DateTime.Now;

            }).ReverseMap();
        }
    }
}

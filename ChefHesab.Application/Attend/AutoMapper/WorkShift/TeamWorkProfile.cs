using AutoMapper;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.TeamWork;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Profiles.WorkShift
{
    public class TeamWorkProfile : Profile
    {
        /// <summary>
        ///
        /// </summary>
        public TeamWorkProfile()
        {
            //CreateMap<TeamWorkModel, TeamWork>().ReverseMap();
            //CreateMap<AddTeamWorkModel, TeamWork>().ReverseMap();
            //CreateMap<EditTeamWorkModel, TeamWork>().ReverseMap();
            CreateMap<SearchTeamWorkModel, TeamWork>().ReverseMap();

            CreateMap<TeamWork, TeamWorkModel>().ReverseMap(); ;
            CreateMap<TeamWork, AddTeamWorkModel>().ReverseMap().AfterMap((src, dest) =>
            {
                dest.InsertDate = System.DateTime.Now;
                dest.InsertUser = src.CurrentUserName;
                dest.IsActive = true;
            }).ReverseMap();
            CreateMap<TeamWork,EditTeamWorkModel >().ReverseMap().AfterMap((src, dest) =>
            {
                dest.UpdateUser = src.CurrentUserName;
                dest.UpdateDate = System.DateTime.Now;

            }).ReverseMap();

        }
    }
}

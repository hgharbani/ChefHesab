using AutoMapper;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.TeamWorkMangementCode;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Profiles.WorkShift
{
    public class TeamWorkMangementCodeProfile : Profile
    {
        /// <summary>
        ///
        /// </summary>
        public TeamWorkMangementCodeProfile()
        {
            //CreateMap<TeamWorkMangementCodeModel, TeamWorkMangementCode>().ReverseMap();    
            //CreateMap<AddTeamWorkMangementCodeModel, TeamWorkMangementCode>().ReverseMap();           
            //CreateMap<EditTeamWorkMangementCodeModel, TeamWorkMangementCode>().ReverseMap();
            CreateMap<SearchTeamWorkMangementCodeModel, TeamWorkMangementCode>().ReverseMap();
           

            CreateMap<TeamWorkMangementCode, TeamWorkMangementCodeModel>().ReverseMap(); ;
            CreateMap<TeamWorkMangementCode, AddTeamWorkMangementCodeModel>().ReverseMap().AfterMap((src, dest) =>
            {
                dest.InsertDate = System.DateTime.Now;
                dest.InsertUser = src.CurrentUserName;
                dest.IsActive = true;
            }).ReverseMap();
            CreateMap<TeamWorkMangementCode,EditTeamWorkMangementCodeModel>().ReverseMap().AfterMap((src, dest) =>
            {
                dest.UpdateUser = src.CurrentUserName;
                dest.UpdateDate = System.DateTime.Now;

            }).ReverseMap();
        }
    }
}

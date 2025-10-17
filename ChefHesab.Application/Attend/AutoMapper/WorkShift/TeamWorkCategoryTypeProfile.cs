using AutoMapper;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.TeamWorkCategoryType;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Profiles.WorkShift
{
    public class TeamWorkCategoryTypeProfile : Profile
    {
        /// <summary>
        ///
        /// </summary>
        public TeamWorkCategoryTypeProfile()
        {
            //CreateMap<TeamWorkCategoryTypeModel, TeamWorkCategoryType>().ReverseMap();    
            //CreateMap<AddTeamWorkCategoryTypeModel, TeamWorkCategoryType>().ReverseMap();           
            //CreateMap<EditTeamWorkCategoryTypeModel, TeamWorkCategoryType>().ReverseMap();
            CreateMap<SearchTeamWorkCategoryTypeModel, TeamWorkCategoryType>().ReverseMap();
            CreateMap<TeamWorkCategoryType, TeamWorkCategoryTypeModel>().ReverseMap(); ;
            CreateMap<TeamWorkCategoryType, AddTeamWorkCategoryTypeModel>().ReverseMap().AfterMap((src, dest) =>
            {
                dest.InsertDate = System.DateTime.Now;
                dest.InsertUser = src.CurrentUserName;
                dest.IsActive = true;
            }).ReverseMap();
            CreateMap< TeamWorkCategoryType,EditTeamWorkCategoryTypeModel > ().ReverseMap().AfterMap((src, dest) =>
            {
                dest.UpdateUser = src.CurrentUserName;
                dest.UpdateDate = System.DateTime.Now;

            }).ReverseMap();
        }
    }
}

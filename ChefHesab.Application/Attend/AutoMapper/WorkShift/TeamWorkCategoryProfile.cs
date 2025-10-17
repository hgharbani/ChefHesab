using AutoMapper;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.TeamWorkCategory;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Profiles.WorkShift
{
    public class TeamWorkCategoryProfile : Profile
    {
        /// <summary>
        ///
        /// </summary>
        public TeamWorkCategoryProfile()
        {
            //CreateMap<TeamWorkCategoryModel, TeamWorkCategory>().ReverseMap();    
            //CreateMap<AddTeamWorkCategoryModel, TeamWorkCategory>().ReverseMap();
            //CreateMap<EditTeamWorkCategoryModel, TeamWorkCategory>().ReverseMap();
            CreateMap<SearchTeamWorkCategoryModel, TeamWorkCategory>().ReverseMap();
            CreateMap<TeamWorkCategory, TeamWorkCategoryModel>().ReverseMap(); ;
            CreateMap<TeamWorkCategory, AddTeamWorkCategoryModel>().ReverseMap().AfterMap((src, dest) =>
            {
                dest.InsertDate = System.DateTime.Now;
                dest.InsertUser = src.CurrentUserName;
                dest.IsActive = true;
            }).ReverseMap();
            CreateMap<TeamWorkCategory,EditTeamWorkCategoryModel>().ReverseMap().AfterMap((src, dest) =>
            {
                dest.UpdateUser = src.CurrentUserName;
                dest.UpdateDate = System.DateTime.Now;

            }).ReverseMap();
        }
    }
}

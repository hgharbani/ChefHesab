using AutoMapper;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.OverTimeDefinition;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Profiles.WorkShift
{
    public class OverTimeDefinitionProfile : Profile
    {
        /// <summary>
        ///
        /// </summary>
        public OverTimeDefinitionProfile()
        {
            //CreateMap<OverTimeDefinitionModel, OverTimeDefinition>().ReverseMap();    
            //CreateMap<AddOverTimeDefinitionModel, OverTimeDefinition>().ReverseMap();           
            //CreateMap<EditOverTimeDefinitionModel, OverTimeDefinition>().ReverseMap();
            CreateMap<SearchOverTimeDefinitionModel, OverTimeDefinition>().ReverseMap();

            CreateMap<OverTimeDefinition, OverTimeDefinitionModel>().ReverseMap(); ;
            CreateMap<OverTimeDefinition, AddOverTimeDefinitionModel>().ReverseMap().AfterMap((src, dest) =>
            {
                dest.InsertDate = System.DateTime.Now;
                dest.InsertUser = src.CurrentUserName;
                dest.IsActive = true;
            }).ReverseMap();
            CreateMap<OverTimeDefinition,EditOverTimeDefinitionModel>().ReverseMap().AfterMap((src, dest) =>
            {
                dest.UpdateUser = src.CurrentUserName;
                dest.UpdateDate = System.DateTime.Now;

            }).ReverseMap();
        }
    }
}

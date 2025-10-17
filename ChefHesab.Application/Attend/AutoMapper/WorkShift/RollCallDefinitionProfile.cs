using AutoMapper;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.RollCallDefinication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Profiles.WorkShift
{
    public class RollCallDefinitionProfile : Profile
    {
        /// <summary>
        ///
        /// </summary>
        public RollCallDefinitionProfile()
        {
            CreateMap<RollCallDefinicationModel, RollCallDefinition>().ReverseMap();
            CreateMap<AddRollCallDefinicationModel, RollCallDefinition>().ReverseMap().AfterMap((src, dest) =>
            {
                dest.InsertDate = System.DateTime.Now;
                dest.InsertUser = src.InsertUser;
                dest.DomainName = src.DomainName;
                dest.IsActive = true;
            }).ReverseMap() ;
            CreateMap<EditRollCallDefinicationModel, RollCallDefinition>().ReverseMap();
            CreateMap<SearchRollCallDefinicationModel, RollCallDefinition>().ReverseMap();
        }
    }
}


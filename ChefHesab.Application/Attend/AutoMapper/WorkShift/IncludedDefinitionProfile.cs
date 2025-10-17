using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.IncludedDefinition;
using AutoMapper;

namespace Ksc.HR.Appication.Profiles.WorkShift
{
    public class IncludedDefinitionProfile : Profile
    {
        /// <summary>
        ///
        /// </summary>
        public IncludedDefinitionProfile()
        {
            CreateMap<IncludedDefinitionModel, IncludedDefinition>();
            CreateMap<IncludedDefinition, IncludedDefinitionModel>();
            CreateMap<AddIncludedDefinitionModel, IncludedDefinition>();
            CreateMap<IncludedDefinition, AddIncludedDefinitionModel>(); 
            CreateMap<EditIncludedDefinitionModel, IncludedDefinition>();
            CreateMap<IncludedDefinition, EditIncludedDefinitionModel>();
        }
    }
}

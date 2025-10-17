using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.RollCallConcept;
using AutoMapper;

namespace Ksc.HR.Appication.Profiles.WorkShift
{
    public class RollCallConceptProfile : Profile
    {
        /// <summary>
        ///
        /// </summary>
        public RollCallConceptProfile()
        {
            CreateMap<RollCallConceptModel, RollCallConcept>();
            CreateMap<RollCallConcept, RollCallConceptModel>();
            CreateMap<AddRollCallConceptModel, RollCallConcept>();
            CreateMap<RollCallConcept, AddRollCallConceptModel>(); 
            CreateMap<EditRollCallConceptModel, RollCallConcept>();
            CreateMap<RollCallConcept, EditRollCallConceptModel>();
        }
    }
}

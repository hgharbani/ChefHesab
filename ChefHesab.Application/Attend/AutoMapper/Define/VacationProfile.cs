using Ksc.Hr.Domain.Entities;
using Ksc.Hr.DTO.Vacation;
using AutoMapper;
namespace Ksc.Hr.Application.Profiles
{
    public class VacationProfile : Profile
    {
        public VacationProfile()
        {
            this.CreateMap<Vacation, VacationDto>();
            // .ForMember(d => d.Vacation, s => s.MapFrom(a => a.Vacation));
            this.CreateMap<VacationDto, Vacation>();
            this.CreateMap<Vacation, AddOrEditVacationDto>();
            this.CreateMap<AddOrEditVacationDto, Vacation>();
            this.CreateMap<Vacation, Vacation>();
            this.CreateMap<Vacation, VacationPairDto>();
            this.CreateMap<VacationPairDto, Vacation>();
        }
    }
}


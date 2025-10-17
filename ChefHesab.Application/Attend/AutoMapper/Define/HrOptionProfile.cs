   using Ksc.Hr.Domain.Entities;
   using Ksc.Hr.DTO.HrOption;
  using AutoMapper;
 namespace Ksc.Hr.Application.Profiles
 {
  public class HrOptionProfile : Profile
 {
  public HrOptionProfile()
 {
  this.CreateMap<HrOption, HrOptionDto>();
  // .ForMember(d => d.HrOption, s => s.MapFrom(a => a.HrOption));
   this.CreateMap<HrOptionDto, HrOption>();
   this.CreateMap<HrOption, AddOrEditHrOptionDto>();
   this.CreateMap<AddOrEditHrOptionDto, HrOption>();
    this.CreateMap<HrOption, HrOption>();
   this.CreateMap<HrOption, HrOptionPairDto>();
    this.CreateMap<HrOptionPairDto, HrOption>();
   }
  }
 }


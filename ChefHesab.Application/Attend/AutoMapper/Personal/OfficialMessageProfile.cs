   using Ksc.Hr.Domain.Entities.Personal;
   using Ksc.Hr.DTO.Personal.OfficialMessage;
  using AutoMapper;
 namespace Ksc.Hr.Application.Profiles.Personal
{
  public class OfficialMessageProfile : Profile
 {
  public OfficialMessageProfile()
 {
  this.CreateMap<OfficialMessage, OfficialMessageDto>();
  // .ForMember(d => d.OfficialMessage, s => s.MapFrom(a => a.OfficialMessage));
   this.CreateMap<OfficialMessageDto, OfficialMessage>();
   this.CreateMap<OfficialMessage, AddOrEditOfficialMessageDto>();
   this.CreateMap<AddOrEditOfficialMessageDto, OfficialMessage>();
    this.CreateMap<OfficialMessage, OfficialMessage>();
   this.CreateMap<OfficialMessage, OfficialMessagePairDto>();
    this.CreateMap<OfficialMessagePairDto, OfficialMessage>();
   }
  }
 }


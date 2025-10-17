using AutoMapper;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.DTO.Personal.MaritalStatus;

namespace Ksc.HR.Appication.Profiles.Personal
{
    public class MaritalStatusProfile : Profile
    {
        public MaritalStatusProfile()
        {
            CreateMap<SearchMaritalStatusDto, MaritalStatus>().ReverseMap();
            CreateMap<AddMaritalStatusDto, MaritalStatus>().ReverseMap();
            CreateMap<EditMaritalStatusDto, MaritalStatus>().ReverseMap();
            CreateMap<ListMaritalStatusDto, MaritalStatus>().ReverseMap();

        }
    }
}

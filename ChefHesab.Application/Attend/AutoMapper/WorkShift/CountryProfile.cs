using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.Country;
using AutoMapper;

namespace Ksc.HR.Appication.Profiles.WorkShift
{
    public class CountryProfile : Profile
    {

        public CountryProfile()
        {
            CreateMap<CountryModel, Country>().ReverseMap();
            CreateMap<Country, AddOrEditCountryModel>().ReverseMap();
        }
    }
}

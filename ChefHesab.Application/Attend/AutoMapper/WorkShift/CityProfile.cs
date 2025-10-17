using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.City;
using AutoMapper;

namespace Ksc.HR.Appication.Profiles.WorkShift
{
    public class CityProfile : Profile
    {
        /// <summary>
        ///
        /// </summary>
        public CityProfile()
        {
            CreateMap<CityModel, City>().ReverseMap();
            CreateMap<AddCityModel, City>().ReverseMap();
            CreateMap<EditCityModel, City>().ReverseMap();
            CreateMap<SearchCityModel, City>().ReverseMap();

            CreateMap<CityTrainingModel, City>().ReverseMap();
        }
    }
}

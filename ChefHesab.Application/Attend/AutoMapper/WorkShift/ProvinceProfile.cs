using AutoMapper;
using DNTPersianUtils.Core.IranCities;
using Ksc.HR.DTO.WorkShift.Province;
using System.Collections.Generic;

namespace Ksc.HR.Appication.Profiles.WorkShift
{
    public class ProvinceProfile : Profile
    {
        public ProvinceProfile()
        {
            CreateMap<ProvinceModel, Province>().ReverseMap();
            CreateMap<AddProvinceModel, Province>().ReverseMap();
            CreateMap<EditProvinceModel, Province>().ReverseMap();
            CreateMap<SearchProvinceModel, Province>().ReverseMap();

        }
    }
}

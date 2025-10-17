using KSC.Common;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.Country;
using KSC.Common.Filters.Models;
using System.Collections.Generic;
using Ksc.HR.DTO.WorkShift.City;

namespace Ksc.HR.Appication.Interfaces.WorkShift
{
    public interface ICountryService
    {
        bool Exists(int id, string name);

        bool Exists(string name);
        List<CountryModel> GetCountries();
        FilterResult<CountryModel> GetCountriesByFilter(FilterRequest Filter);
        KscResult AddCountry(AddOrEditCountryModel model);
        KscResult UpdateCountry(AddOrEditCountryModel model);
        KscResult RemoveCountry(AddOrEditCountryModel model);
        AddOrEditCountryModel GetEntity(int id);
        List<SearchCountryModel> GetCountryByKendoFilter(CityFilterRequest Filter);
    }
}
using KSC.Common;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.City;
using Ksc.HR.DTO.WorkShift.Province;
using Ksc.HR.DTO.WorkShift.WorkTime;
using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.DTO.WorkFlow.BaseFile;

namespace Ksc.HR.Appication.Interfaces.WorkShift
{
    public interface ICityService
    {
        bool Exists(int id, string name);
        bool Exists(string name);
        List<CityModel> GetCity();

        List<CityTrainingModel> GetCityTraining();
        FilterResult<CityModel> GetCityByFilter(FilterRequest Filter);
        City GetOne(int id);
        EditCityModel GetForEdit(int id);

        Task<KscResult> AddCity(AddCityModel model);
        Task<KscResult> UpdateCity(EditCityModel model);
        KscResult RemoveCity(EditCityModel model);
        List<SearchCityModel> GetCitiesByKendoFilter(FilterRequest Filter);
        /// <summary>
        /// شهر به همراه استان و کشور نمایش داده می شود
        /// </summary>
        /// <param name="Filter"></param>
        /// <returns></returns>
        FilterResult<CityModel> GetCityWithProviceCountryByFilter(FilterRequest Filter);
        FilterResult<CityModel> GetCityWithProviceCountryValidForMissionByFilter(FilterRequest Filter);
        FilterResult<SearchCityModel> GetWorkCitiesProviceByKendoFilter(FilterRequest Filter);
        FilterResult<CityModel> GetCityByFilterBooklet(FilterRequest Filter);
        List<AutomCompleteModel> AutoComplete(string text);
        List<AutomCompleteModel> GetByIds(List<int> ids);
    }
}

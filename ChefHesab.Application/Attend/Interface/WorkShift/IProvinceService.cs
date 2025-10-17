using KSC.Common;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.Province;
using KSC.Common.Filters.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Interfaces.WorkShift
{
    public interface IProvinceService
    {
        bool Exists(int id, string name);

        bool Exists(string name);
                Province GetOne(int id);

        AddProvinceModel GetForEdit(int id);
        FilterResult<ProvinceModel> GetProvinces(FilterRequest Filter);
        //FilterResult<ProvinceModel> GetProvinces(FilterRequest Filter);
        List<SearchProvinceModel> GetProvincesByKendoFilter(FilterRequest Filter);
        List<SearchProvinceModel> GetProvincesByKendoFilterBycountryId(SearchProvinceByCountryId Filter);
        Task<KscResult> AddProvince(AddProvinceModel model);
        Task<KscResult> UpdateProvince(AddProvinceModel model);
        KscResult RemoveProvince(EditProvinceModel model);


    }
}
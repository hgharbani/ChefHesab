using KSC.Common;
using Ksc.HR.DTO.WorkShift.WorkCompanySetting;
using KSC.Common.Filters.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Interfaces.WorkShift
{
    public interface IWorkCompanySettingServices
    {
        Task<KscResult> AddWorkCompanySetting(AddWorkCompanySettingModel model);
        //void Exists(int id, int? WorktimeId, int? shiftconceptId);
        //void Exists(int? WorktimeId, int? shiftconceptId);
        FilterResult<SearchWorkCompanySettingModel> GetSettingByModel(FilterRequest Filter);
        Task<KscResult> UpdateWorkCompanySetting(EditWorkCompanySettingModel model);
        int? GetLastSetting();
    }
}
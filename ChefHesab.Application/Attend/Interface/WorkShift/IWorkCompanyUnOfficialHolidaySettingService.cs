using KSC.Common;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.WorkCompanyUnOfficialHolidaySetting;
using KSC.Common.Filters.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Interfaces.WorkShift
{
    public interface IWorkCompanyUnOfficialHolidaySettingService
    {
        Task<KscResult> AddUnOfficialHolidaySetting(AddWorkCompanyUnOfficialHolidaySettingModel model);
        
        EditWorkCompanyUnOfficialHolidaySettingModel GetEntity(int id);
        EditWorkCompanyUnOfficialHolidaySettingModel GetForEdit(int id);
        WorkCompanyUnOfficialHolidaySetting GetOne(int id);
        List<WorkCompanyUnOfficialHolidaySettingModel> GetWorkCompanyUnOfficialHolidaySettings();
        FilterResult<WorkCompanyUnOfficialHolidaySettingModel> GetWorkCompanyUnOfficialHolidaySettingsByFilter(WorkCompanyUnOfficialHolidaySettingModel Filter);
        Task<KscResult> RemoveWorkCompanyUnOfficialHolidaySetting(EditWorkCompanyUnOfficialHolidaySettingModel model);
        Task<KscResult> UpdateWorkCompanyUnOfficialHolidaySetting(EditWorkCompanyUnOfficialHolidaySettingModel model);
    }
}
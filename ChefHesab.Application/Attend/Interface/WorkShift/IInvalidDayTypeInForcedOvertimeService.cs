using KSC.Common;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.InvalidDayTypeInForcedOvertime;
using Ksc.HR.DTO.WorkShift.Province;
using Ksc.HR.DTO.WorkShift.WorkTime;
using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Interfaces.WorkShift
{
    public interface IInvalidDayTypeInForcedOvertimeService
    {
        //void ExistsByTitle(int id, string name);
        //void ExistsByTitle(string name);
        //List<InvalidDayTypeInForcedOvertimeModel> GetInvalidDayTypeInForcedOvertime();
        FilterResult<InvalidDayTypeInForcedOvertimeModel> GetInvalidDayTypeInForcedOvertimeByFilter(FilterRequest Filter);
        //InvalidDayTypeInForcedOvertime GetOne(int id);
        EditInvalidDayTypeInForcedOvertimeModel GetForEdit(int id);

        Task<KscResult> AddInvalidDayTypeInForcedOvertime(AddInvalidDayTypeInForcedOvertimeModel model);
        Task<KscResult> UpdateInvalidDayTypeInForcedOvertime(EditInvalidDayTypeInForcedOvertimeModel model);
        //Task<KscResult> RemoveInvalidDayTypeInForcedOvertime(EditInvalidDayTypeInForcedOvertimeModel model);
        //List<SearchInvalidDayTypeInForcedOvertimeModel> GetInvalidDayTypeInForcedOvertimesByKendoFilter(FilterRequest Filter);

    }
}

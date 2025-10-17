using KSC.Common;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.WorkDayType;
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
    public interface IWorkDayTypeService
    {
        bool ExistsByTitle(int id, string name);
        bool ExistsByTitle(string name);
        List<WorkDayTypeModel> GetWorkDayType();
        FilterResult<WorkDayTypeModel> GetWorkDayTypeByFilter(FilterRequest Filter);
        WorkDayType GetOne(int id);
        EditWorkDayTypeModel GetForEdit(int id);

        Task<KscResult> AddWorkDayType(AddWorkDayTypeModel model);
        Task<KscResult> UpdateWorkDayType(EditWorkDayTypeModel model);
        Task<KscResult> RemoveWorkDayType(EditWorkDayTypeModel model);
        List<SearchWorkDayTypeModel> GetWorkDayTypesByKendoFilter(FilterRequest Filter);

    }
}

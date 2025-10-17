using KSC.Common;
using Ksc.HR.Domain.Entities.Workshift;
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
    public interface IWorkTimeService
    {
        void Exists(int id, string name);
        void Exists(string name);
        List<WorkTimeModel> GetWorkTime();
        FilterResult<WorkTimeModel> GetWorktimeByFilter(FilterRequest Filter);
        WorkTime GetOne(int id);
        EditWorkTimeModel GetForEdit(int id);
        List<SearchWorkTimeModel> GetWorkByKendoFilter(FilterRequest Filter);
        Task<KscResult> AddWorkTime(AddWorkTimeModel model);
        Task<KscResult> UpdateWorkTime(EditWorkTimeModel model);
        Task<KscResult> RemoveWorkTime(EditWorkTimeModel model);
        List<SearchWorkTimeModel> GetByKendoFilterForRelocation(FilterRequest Filter);
    }
}

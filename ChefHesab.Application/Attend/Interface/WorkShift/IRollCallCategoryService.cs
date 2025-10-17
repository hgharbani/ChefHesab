using KSC.Common;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.RollCallCategory;
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
    public interface IRollCallCategoryService
    {
        bool ExistsByTitle(int id, string name);
        bool ExistsByTitle(string name);
        List<RollCallCategoryModel> GetRollCallCategory();
        FilterResult<RollCallCategoryModel> GetRollCallCategoryByFilter(FilterRequest Filter);
        RollCallCategory GetOne(int id);
        EditRollCallCategoryModel GetForEdit(int id);

        Task<KscResult> AddRollCallCategory(AddRollCallCategoryModel model);
        Task<KscResult> UpdateRollCallCategory(EditRollCallCategoryModel model);
        Task<KscResult> RemoveRollCallCategory(EditRollCallCategoryModel model);
        List<SearchRollCallCategoryModel> GetRollCallCategorysByKendoFilter(FilterRequest Filter);

    }
}

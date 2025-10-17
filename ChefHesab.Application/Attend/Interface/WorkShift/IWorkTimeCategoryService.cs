using KSC.Common;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.WorkTimeCategory;
using KSC.Common.Filters.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Interfaces.WorkShift
{
    public interface IWorkTimeCategoryService
    {
        void ExistsTitle(int id, string title);

        void ExistsTitle(string title);

        void ExistsCode(string code);

        void ExistsCode(int id, string code);

        List<WorkTimeCategoryModel> GetWorkTimeCategories();
        FilterResult<WorkTimeCategoryModel> GetWorktimeCategories(FilterRequest Filter);
        WorkTimeCategory GetOne(int id);
        List<SearchWorkTimeCategoryModel> GetWorkTimeCategoriesByKendoFilter(FilterRequest Filter);
        AddOrEditWorkTimeCategoryModel GetForEdit(int id);

        Task<KscResult> AddWorkTimeCategory(AddOrEditWorkTimeCategoryModel model);
        Task<KscResult> UpdateWorkTimeCategory(AddOrEditWorkTimeCategoryModel model);
        Task<KscResult> RemoveWorkTimeCategory(AddOrEditWorkTimeCategoryModel model);


    }
}
using KSC.Common;
using Ksc.HR.Domain.Entities.ODSViews;
using Ksc.HR.DTO.ODSViews.ViewMisJobCategory;
using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Interfaces.ODSViews
{
    public interface IViewMisJobCategoryService
    {
        FilterResult<SearchViewMisJobCategoryModel> GetViewMisJobCategoryByKendoFilter(FilterRequest Filter);
        List<SearchViewMisJobCategoryModel> GetAllViewMisJobCategoryByKendoFilter(FilterRequest Filter);
        List<SearchViewMisJobCategoryModel> GetCodeCategoryJobCategoryByKendoFilter(FilterRequest Filter);
    }
}

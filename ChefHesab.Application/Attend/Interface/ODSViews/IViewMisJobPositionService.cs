using KSC.Common;
using Ksc.HR.Domain.Entities.ODSViews;
using Ksc.HR.DTO.ODSViews.ViewMisJobPosition;
using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.DTO.WorkFlow.BaseFile;
using Ksc.HR.DTO.ODSViews.ViewMisJobCategory;

namespace Ksc.HR.Appication.Interfaces.ODSViews
{
    public interface IViewMisJobPositionService
    {
        List<SearchViewMisJobStausCodeModel> GetAllViewMisJobStatusByKendoFilter(FilterRequest Filter);
        Task<List<AutomCompleteModel>> GetViewMisJobPositionForAutoCompleteWF(AutoCompleteInputModel inputModel);
        Task<FilterResult<AutomCompleteModel>> GetViewMisJobPositionForConditionAutoComplete(AutoCompleteInputModel inputModel);
        Task<AutomCompleteModel> GetByJobPositionCode(string jobPositionCode);


    }
}

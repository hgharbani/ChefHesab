using KSC.Common;
using Ksc.HR.Domain.Entities.ODSViews;
using Ksc.HR.DTO.ODSViews.ViewMisUserDefinition;
using Ksc.HR.DTO.WorkFlow.BaseFile;
using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Interfaces.ODSViews
{
    public interface IViewMiSUserDefinitionService
    {
        FilterResult<SearchViewMisUserDefinitionModel> GetViewMiSUserDefinitionByKendoFilter(FilterRequest Filter);
        Task<List<AutomCompleteModel>> GetMiSUserDefinitionForAutoCompleteWF(AutoCompleteInputModel inputModel);
    }
}

using KSC.Common;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.IncludedDefinition;
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
    public interface IIncludedDefinitionService
    {
        bool ExistsByTitle(int id, string name);
        bool ExistsByTitle(string name);
        List<IncludedDefinitionModel> GetIncludedDefinition();
        FilterResult<IncludedDefinitionModel> GetIncludedDefinitionByFilter(FilterRequest Filter);
        IncludedDefinition GetOne(int id);
        EditIncludedDefinitionModel GetForEdit(int id);

        Task<KscResult> AddIncludedDefinition(AddIncludedDefinitionModel model);
        Task<KscResult> UpdateIncludedDefinition(EditIncludedDefinitionModel model);
        Task<KscResult> RemoveIncludedDefinition(EditIncludedDefinitionModel model);
        List<SearchIncludedDefinitionModel> GetIncludedDefinitionsByKendoFilter(FilterRequest Filter);
        List<SearchIncludedDefinitionModel> GetIncludedSalaryDeductionByKendoFilter(FilterRequest Filter);
        List<SearchIncludedDefinitionModel> GetIncludedRewardDeductionByKendoFilter(FilterRequest Filter);
        List<SearchIncludedDefinitionModel> GetIncludedCodeForMultiSelectByKendoFilter(FilterRequest Filter);

    }
}

using KSC.Common;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.OverTimeDefinition;
using KSC.Common.Filters.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Interfaces.WorkShift
{
    public interface IOverTimeDefinitionService
    {
        void ExistsByCode(int id, string code);
        void Exists(int id, string name);
        void Exists(string name);
        List<OverTimeDefinitionModel> GetOverTimeDefinitions();
        FilterResult<OverTimeDefinitionModel> GetOverTimeDefinitionsByFilter(FilterRequest Filter);
        OverTimeDefinition GetOne(int id);
        List<SearchOverTimeDefinitionModel> GetOverTimeDefinitionsByKendoFilter(FilterRequest Filter);
        EditOverTimeDefinitionModel GetForEdit(int id);

        Task<KscResult> AddOverTimeDefinition(AddOverTimeDefinitionModel model);
        Task<KscResult> UpdateOverTimeDefinition(EditOverTimeDefinitionModel model);
        Task<KscResult> RemoveOverTimeDefinition(EditOverTimeDefinitionModel model);
       
     
    }
}
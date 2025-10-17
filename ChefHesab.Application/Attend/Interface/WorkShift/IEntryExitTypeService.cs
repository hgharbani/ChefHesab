using KSC.Common;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.EntryExitType;
using KSC.Common.Filters.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ksc.HR.DTO.WorkFlow.BaseFile;

namespace Ksc.HR.Appication.Interfaces.WorkShift
{
    public interface IEntryExitTypeService
    {
        bool ExistsByCode(int id, string code);
        bool Exists(int id, string name);

        bool Exists(string name);
        List<EntryExitTypeModel> GetEntryExitTypes();
        FilterResult<EntryExitTypeModel> GetEntryExitTypesByFilter(FilterRequest Filter);
        EntryExitType GetOne(int id);
        AddOrEditEntryExitTypeModel GetForEdit(int id);

        KscResult AddEntryExitType(AddOrEditEntryExitTypeModel model);
        KscResult UpdateEntryExitType(AddOrEditEntryExitTypeModel model);
        KscResult RemoveEntryExitType(AddOrEditEntryExitTypeModel model);
        Task<List<AutomCompleteModel>> GetEntryExitTypes_AutoComplete();
    }
}
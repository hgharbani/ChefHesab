using KSC.Common;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.ShiftConcept;
using KSC.Common.Filters.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Interfaces.WorkShift
{
    public interface IShiftConceptService
    {
        void ExistsByCode(int id, string code);
        void Exists(int id, string name);

        void Exists(string name);
        List<ShiftConceptModel> GetShiftConcepts();
        FilterResult<ShiftConceptModel> GetShiftConceptsByFilter(FilterRequest Filter);
        ShiftConcept GetOne(int id);
        List<SearchShiftConceptModel> GetShiftConceptsByKendoFilter(FilterRequest Filter);
        AddOrEditShiftConceptModel GetForEdit(int id);

        Task<KscResult> AddShiftConcept(AddOrEditShiftConceptModel model);
        Task<KscResult> UpdateShiftConcept(AddOrEditShiftConceptModel model);
        Task<KscResult> RemoveShiftConcept(AddOrEditShiftConceptModel model);
       
     
    }
}
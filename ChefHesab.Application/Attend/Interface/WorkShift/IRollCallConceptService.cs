using KSC.Common;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.RollCallConcept;
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
    public interface IRollCallConceptService
    {
        bool ExistsByTitle(int id, string name);
        bool ExistsByTitle(string name);
        List<RollCallConceptModel> GetRollCallConcept();
        FilterResult<RollCallConceptModel> GetRollCallConceptByFilter(FilterRequest Filter);
        RollCallConcept GetOne(int id);
        EditRollCallConceptModel GetForEdit(int id);

        Task<KscResult> AddRollCallConcept(AddRollCallConceptModel model);
        Task<KscResult> UpdateRollCallConcept(EditRollCallConceptModel model);
        Task<KscResult> RemoveRollCallConcept(EditRollCallConceptModel model);
        List<SearchRollCallConceptModel> GetRollCallConceptsByKendoFilter(FilterRequest Filter);

    }
}

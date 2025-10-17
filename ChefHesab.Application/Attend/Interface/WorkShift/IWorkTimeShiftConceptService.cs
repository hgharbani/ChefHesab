using KSC.Common;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.Personal.Employee;
using Ksc.HR.DTO.WorkShift.WorkTimeShiftConcept;
using KSC.Common.Filters.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Interfaces.WorkShift
{
    public interface IWorkTimeShiftConceptService
    {
        FilterResult<SearchWorkTimeShiftConceptModel> GetWorkTimeShiftConceptByModel(FilterRequest Filter);
    }
}
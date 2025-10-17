using KSC.Common;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.UnofficialHolidayReason;
using Ksc.HR.DTO.WorkShift.WorkCompanyUnOfficialHolidaySetting;
using KSC.Common.Filters.Models;
using System.Collections.Generic;

namespace Ksc.HR.Appication.Interfaces.WorkShift
{
    public interface IUnofficialHolidayReasonService
    {
        List<SearchUnofficialHolidayReasonModel> GetUnofficialHolidayReasonsByKendoFilter(FilterRequest Filter);
    }
}
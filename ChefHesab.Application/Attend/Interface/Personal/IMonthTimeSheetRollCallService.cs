using Ksc.HR.DTO.Personal.MonthTimeSheet;
using Ksc.HR.DTO.Personal.MonthTimeSheetRollCall;
using KSC.Common;
using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Interfaces.Personal
{
    public interface IMonthTimeSheetRollCallService
    {
        Task<FilterResult<SearchTimeSheetRollCallModel>> GetMonthTimeSheetRollCallByKendoFilter(SearchTimeSheetRollCallModel Filter);
    }
}

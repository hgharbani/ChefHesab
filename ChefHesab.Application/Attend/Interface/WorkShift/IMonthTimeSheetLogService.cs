using Ksc.HR.DTO.WorkShift.MonthTimeSheetLog;
using KSC.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Interfaces.WorkShift
{
    public interface IMonthTimeSheetLogService
    {
        Task<KscResult> CheckLogsByYearMonthStep(SearchLogModel model);
    }
}

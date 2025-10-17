

using Ksc.HR.DTO.WorkShift.RollCallWorkTimeMonthSetting;
using KSC.Common;
using KSC.Common.Filters.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Interfaces.WorkShift
{
    public interface IRollCallWorkTimeMonthSettingService
    {
        FilterResult<SearchRollCallWorkTimeMonthSettingModel> GetRollCallWorkTimeMonthSettingByFilter(SearchRollCallWorkTimeMonthSettingModel Filter);
    }
}

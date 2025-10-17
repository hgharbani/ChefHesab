using KSC.Common;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.TimeShiftSetting;
using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Interfaces.WorkShift
{
    public interface ITimeShiftSettingService
    {
        bool Exists(int id, string name);
        bool Exists(string name);
        List<TimeShiftSettingModel> GetTimeShiftSetting();
        FilterResult<TimeShiftSettingModel> GetTimeShiftSettingByFilter(TimeShiftSettingModel Filter);
        TimeShiftSetting GetOne(int id);
        EditTimeShiftSettingModel GetForEdit(int id);
        List<SearchTimeShiftSettingModel> GetWorkByKendoFilter(FilterRequest Filter);
        Task<KscResult> AddTimeShiftSetting(AddTimeShiftSettingModel model);
        Task<KscResult> UpdateTimeShiftSetting(EditTimeShiftSettingModel model);
        KscResult RemoveTimeShiftSetting(EditTimeShiftSettingModel model);
        List<ForcedOverTimeModel> GetDataForcedOverTime(DateTime date);
        int GetTotalWorkHourInDayForRequestSystem(string month);
    }
}

using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.DTO.WorkShift.TimeSheetSetting;
using KSC.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Appication.Interfaces.WorkShift
{
    public interface ITimeSheetSettingService
    {
        TimeSheetSetting GetOne();
        EditTimeSheetSettingModel GetForEdit();
        Task<KscResult> UpdateTimeSheetSetting(EditTimeSheetSettingModel model);

    }
}

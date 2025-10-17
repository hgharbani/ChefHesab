using KSC.Domain;
using Ksc.HR.Domain.Entities.Workshift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.WorkShift
{
    public interface ITimeSheetSettingRepository : IRepository<TimeSheetSetting, int>
    {
        TimeSheetSetting GetTimeSheetSettingActive();
        Task<TimeSheetSetting> GetTimeSheetSettingActiveAsync();
    }
}

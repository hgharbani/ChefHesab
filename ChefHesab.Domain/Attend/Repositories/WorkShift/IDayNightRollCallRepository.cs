using KSC.Domain;
using Ksc.HR.Domain.Entities.Workshift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Share.Model.DayNightRollCall;

namespace Ksc.HR.Domain.Repositories.WorkShift
{
    public interface IDayNightRollCallRepository : IRepository<DayNightRollCall, int>
    {
        IQueryable<DayNightRollCallSettingModel> GetDayNightRollCallSetting();
    }
}

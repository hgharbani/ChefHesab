using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories.WorkShift;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Share.Model.DayNightRollCall;

namespace Ksc.HR.Data.Persistant.Repositories.WorkShift
{
    public class DayNightRollCallRepository : EfRepository<DayNightRollCall, int>, IDayNightRollCallRepository
    {
        private readonly KscHrContext _kscHrContext;
        public DayNightRollCallRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<DayNightRollCallSettingModel> GetDayNightRollCallSetting()
        {
            var query = from roll in _kscHrContext.DayNightRollCalls.Where(x=>x.IsActive)
                        join time in _kscHrContext.DayNightSettingTimes.Where(x => x.IsActive)
                        on roll.DayNightSettingTimeId equals time.Id
                        select new DayNightRollCallSettingModel()
                        {
                            StartTime = time.StartTime,
                            EndTime = time.EndTime,
                            RollCallDefinitionId = roll.RollCallDefinitionId,
                            DayNumber = roll.DayNumber,
                            IsDay = time.IsDay
                        };
            return query;

        }

    }
}

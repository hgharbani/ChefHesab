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

namespace Ksc.HR.Data.Persistant.Repositories.WorkShift
{
    public class DayTimeSettingRepository : EfRepository<DayTimeSetting, int>, IDayTimeSettingRepository
    {
        private readonly KscHrContext _kscHrContext;
        public DayTimeSettingRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext= KscHrContext;
        }
        public IQueryable<DayTimeSetting> GetQueryable()
        {
            var query = _kscHrContext.DayTimeSettings.AsQueryable();
            return query;

        }
    }
}

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
    public class RollCallWorkTimeMonthSettingRepository : EfRepository<RollCallWorkTimeMonthSetting, int>, IRollCallWorkTimeMonthSettingRepository
    {
        private readonly KscHrContext _kscHrContext;
        public RollCallWorkTimeMonthSettingRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<RollCallWorkTimeMonthSetting> GetActiveData()
        {
            var result = _kscHrContext.RollCallWorkTimeMonthSettings.Where(x => x.IsActive);
            return result;
        }
    }
}

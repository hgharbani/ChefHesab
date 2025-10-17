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
    public class TimeSheetSettingRepository : EfRepository<TimeSheetSetting, int>, ITimeSheetSettingRepository
    {
        private readonly KscHrContext _kscHrContext;
        public TimeSheetSettingRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public TimeSheetSetting GetTimeSheetSettingActive()
        {
            return _kscHrContext.TimeSheetSettings.SingleOrDefault(x => x.IsActive);
        }
        public async Task<TimeSheetSetting> GetTimeSheetSettingActiveAsync()
        {
            return await _kscHrContext.TimeSheetSettings.FirstOrDefaultAsync(x => x.IsActive);
        }
     

    }
}

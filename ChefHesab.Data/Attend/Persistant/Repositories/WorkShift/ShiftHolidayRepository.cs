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
    public class ShiftHolidayRepository : EfRepository<ShiftHoliday, int>, IShiftHolidayRepository
    {
        private readonly KscHrContext _kscHrContext;
        public ShiftHolidayRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public async Task<ShiftHoliday> GetShiftHolidayByWorkCompanySettingDayNumber(int workCompanySettingId, int dayNumber)
        {
            return await _kscHrContext.ShiftHolidays.FirstOrDefaultAsync(x => x.WorkCompanySettingId == workCompanySettingId && x.DayNumber == dayNumber);
        }
        public IQueryable<ShiftHoliday> GetShiftHolidayActive()
        {
            return _kscHrContext.ShiftHolidays.Where(x => x.IsActive);
        }
    }
}

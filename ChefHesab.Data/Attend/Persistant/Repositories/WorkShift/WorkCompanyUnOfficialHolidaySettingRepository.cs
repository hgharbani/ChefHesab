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
    public class WorkCompanyUnOfficialHolidaySettingRepository : EfRepository<WorkCompanyUnOfficialHolidaySetting, int>, IWorkCompanyUnOfficialHolidaySettingRepository
    {
        private readonly KscHrContext _kscHrContext;
        public WorkCompanyUnOfficialHolidaySettingRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<WorkCompanyUnOfficialHolidaySetting> GetAllWithInclude()
        {
            var result = GetAll().AsQueryable().Include(x => x.WorkCalendar).Include(x => x.UnofficialHolidayReason).Include(x => x.WorkCompanyUnOfficialHolidayJobCategories);
            return result;
        }

        public WorkCompanyUnOfficialHolidaySetting GetOneWithInclude(int id)
        {
            var result = _kscHrContext.WorkCompanyUnOfficialHolidaySettings.Include(x => x.WorkCompanyUnOfficialHolidayJobCategories).AsQueryable()
                .FirstOrDefault(x => x.Id == id);
            return result;
        }
        public async Task<WorkCompanyUnOfficialHolidaySetting> GetWorkCompanyUnOfficialHolidaySettingActive(int workCompanySettingId, int workCalendarId)
        {
            var result = await _kscHrContext.WorkCompanyUnOfficialHolidaySettings.AsNoTracking().Include(x=>x.WorkCompanyUnOfficialHolidayJobCategories)
                .FirstOrDefaultAsync(x => x.IsActive == true && x.WorkCompanySettingId == workCompanySettingId && x.WorkCalendarId == workCalendarId);
            return result;
        }
    }
}

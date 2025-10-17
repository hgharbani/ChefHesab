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
    public class WorkCompanySettingRepository : EfRepository<WorkCompanySetting, int>, IWorkCompanySettingRepository
    {
        private readonly KscHrContext _kscHrContext;
        public WorkCompanySettingRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IEnumerable<WorkCompanySetting> GetWorkCompanySettingIncluded()
        {
            return _kscHrContext.WorkCompanySettings.Include(x => x.WorkTimeShiftConcept).ThenInclude(x => x.WorkTime).Include(x => x.WorkTimeShiftConcept).ThenInclude(x => x.ShiftConcept)
                                .Include(a => a.WorkCity).ThenInclude(a => a.City).Include(a => a.WorkCity).ThenInclude(x => x.Company)
                .AsQueryable();
        }

        
        public async Task<WorkCompanySetting> GetWorkCompanySettingByWorkCityIdWorkTimeShiftConceptId(int workCityId, int workTimeShiftConceptId)
        {
            return await _kscHrContext.WorkCompanySettings.Include(x => x.TimeShiftSettings).AsNoTracking().FirstAsync(x => x.WorkCityId == workCityId && x.WorkTimeShiftConceptId == workTimeShiftConceptId);
        }

        public IQueryable<WorkCompanySetting> GetAllWorkCompanySettingByWorkCityIdWorkTimeShiftConceptId(List<int> workCityIds, List<int> workTimeShiftConceptIds)
        {
            return  _kscHrContext.WorkCompanySettings.Include(x => x.TimeShiftSettings).Where(x => workCityIds.Contains(x.WorkCityId) && workTimeShiftConceptIds.Contains(x.WorkTimeShiftConceptId)).AsQueryable();
        } 
        public WorkCompanySetting GetWorkCompanySetting(int id)
        {
            return  _kscHrContext.WorkCompanySettings.Where(x=>x.Id==id).Include(x => x.WorkTimeShiftConcept).FirstOrDefault();
        }
    }
}

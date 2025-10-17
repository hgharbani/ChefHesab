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
    public class WorkTimeRepository : EfRepository<WorkTime, int>, IWorkTimeRepository
    {
        private readonly KscHrContext _kscHrContext;
        public WorkTimeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IEnumerable<WorkTime> GetWorkTimes()
        {
            return _kscHrContext.WorkTimes.Include(a => a.WorkTimeCategory).Include(a => a.WorkGroups).Include(x => x.WorkTimeShiftConcepts).ThenInclude(x => x.ShiftConcept).AsQueryable();
        }
        public IEnumerable<WorkTime> GetAllActive()
        {
            return _kscHrContext.WorkTimes.AsQueryable().Where(x=>x.IsActive);
        }
        public IQueryable<WorkTime> GetAllByIds(List<int> ids)
        {
            return _kscHrContext.WorkTimes.Include(a => a.WorkTimeCategory).Include(a => a.WorkGroups).Include(x => x.WorkTimeShiftConcepts).ThenInclude(x => x.ShiftConcept).AsQueryable();
        }

        public WorkTime GetWorkTimeByCode(string code)
        {
            return _kscHrContext.WorkTimes.FirstOrDefault(x => x.Code == code);
        }
        public WorkTime GetWorkTimeByIdAsNoTracking(int id)
        {
            return _kscHrContext.WorkTimes.Where(x => x.Id == id).AsNoTracking().FirstOrDefault();
        }
        public WorkTime GetWorkTimeInAttendAbsenceItemAsNoTracking(int employeeId, int workCalendarId)
        {
            return _kscHrContext.EmployeeAttendAbsenceItems.Where(x => x.EmployeeId == employeeId && x.WorkCalendarId == workCalendarId).Include(x => x.WorkTime).AsNoTracking().FirstOrDefault().WorkTime;
        }
        public IQueryable<WorkTime> GetWorkTimesAsNoTracking()
        {
            return _kscHrContext.WorkTimes.Include(a => a.WorkGroups)
                .Include(x => x.WorkTimeShiftConcepts).ThenInclude(x => x.ShiftConcept)
                .ThenInclude(x => x.ShiftConceptDetails).AsNoTracking();
        }
    }
}

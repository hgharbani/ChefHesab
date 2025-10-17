using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Repositories.Personal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.Personal
{
    public class EmployeeEducationTimeRepository : EfRepository<EmployeeEducationTime, int>, IEmployeeEducationTimeRepository
    {
        private readonly KscHrContext _kscHrContext;
        public EmployeeEducationTimeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<EmployeeEducationTime> GetActiveEmployeeEducationTimeByEmployeeIdWorkCalendar(int employeeId, int workCalendarId)
        {
            return _kscHrContext.EmployeeEducationTimes.Where(x => x.IsDeleted == false && x.EmployeeId == employeeId && x.WorkCalendarId == workCalendarId).AsNoTracking(); //
        }

    }
}

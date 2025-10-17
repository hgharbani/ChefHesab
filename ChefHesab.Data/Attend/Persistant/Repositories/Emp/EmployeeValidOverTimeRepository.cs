using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Domain.Entities;
using Ksc.HR.Domain.Repositories.EmployeeBase;

using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Share.Model.emp;
using Ksc.HR.Domain.Entities.Emp;
using Ksc.HR.Domain.Repositories.Emp;
using Ksc.HR.Domain.Entities.Workshift;

namespace Ksc.HR.Data.Persistant.Repositories.Emp
{
    public class EmployeeValidOverTimeRepository : EfRepository<EmployeeValidOverTime, long>, IEmployeeValidOverTimeRepository
    {
        private readonly KscHrContext _kscHrContext;
        public EmployeeValidOverTimeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<EmployeeValidOverTime> GetAllQueryable()
        {
            return _kscHrContext.EmployeeValidOverTime.AsQueryable();

        }
        public IQueryable<EmployeeValidOverTime> GetEmployeeValidOverTimeByEmployeeId(int employeeId)
        {
            return _kscHrContext.EmployeeValidOverTime.Where(x => x.EmployeeId == employeeId).AsQueryable();
        }
        public IQueryable<EmployeeValidOverTime> GetEmployeeValidOverTimeByWorkCalendarId(int workCalendarId)
        {
            return _kscHrContext.EmployeeValidOverTime.Where(x => x.WorkCalendarId == workCalendarId);
        }


        public bool ValidOverTimeByEmployeeId(int employeeId, int workCalnedarId)
        {
            return _kscHrContext.EmployeeValidOverTime.Any(x => !x.IsDeleted && x.EmployeeId == employeeId && x.WorkCalendarId == workCalnedarId);
        }

    }
}

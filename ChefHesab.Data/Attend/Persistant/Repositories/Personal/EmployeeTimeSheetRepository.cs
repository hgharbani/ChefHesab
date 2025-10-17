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
using Microsoft.Data.SqlClient;
using System.Data;

namespace Ksc.HR.Data.Persistant.Repositories.Personal
{
    public class EmployeeTimeSheetRepository : EfRepository<EmployeeTimeSheet, int>, IEmployeeTimeSheetRepository
    {
        private readonly KscHrContext _kscHrContext;
        public EmployeeTimeSheetRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        //public IQueryable<EmployeeTimeSheet> GetEmployeeTimeSheetTrainingOverTime(int yearMonth)
        //{
        //    var query = _kscHrContext.EmployeeTimeSheets.Where(x => x.YearMonth == yearMonth && (x.TrainingOverTime != null)).AsNoTracking();
        //    return query;
        //}

        public IQueryable<EmployeeTimeSheet> GetEmployeeTimeSheetExcessOverTime(int yearMonth)
        {
            var query = _kscHrContext.EmployeeTimeSheets.Where(x => x.YearMonth == yearMonth && (x.ExcessOverTime > 0 || x.AverageBalanceOverTime != null || x.TrainingOverTime != null)).AsNoTracking();
            return query;
        }

        public IQueryable<EmployeeTimeSheet> GetEmployeeTimeSheet(int yearMonth)
        {
            var query = _kscHrContext.EmployeeTimeSheets.Where(x => x.YearMonth == yearMonth).AsNoTracking();
            return query;
        }

        public IQueryable<EmployeeTimeSheet> GetEmployeeTimeSheetQueryable(int yearMonth)
        {
            var query = _kscHrContext.EmployeeTimeSheets.Where(x => x.YearMonth == yearMonth).AsQueryable();
            return query;
        }

        public IQueryable<EmployeeTimeSheet> GetEmployeeTimeSheetByRelated(int yearMonth)
        {
            var query = _kscHrContext.EmployeeTimeSheets.Include(a => a.Employee)
                .ThenInclude(a => a.TeamWork)
                .ThenInclude(a => a.OverTimeDefinition)
                .Where(x => x.YearMonth == yearMonth && x.ExcessOverTime != 0).AsNoTracking();
            return query;
        }

        public IQueryable<EmployeeTimeSheet> GetEmployeeTimeSheetByYearMonthAndEmployeeId(int yearMonth, List<int> EmployeeIds)
        {
            var query = _kscHrContext.EmployeeTimeSheets.Where(x => x.YearMonth == yearMonth && EmployeeIds.Contains(x.EmployeeId)).AsNoTracking();
            return query;
        }
    }
}

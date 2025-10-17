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
    public class AnalysisEmployeeAttendAbsenceItemRepository : EfRepository<AnalysisEmployeeAttendAbsenceItem, long>, IAnalysisEmployeeAttendAbsenceItemRepository
    {
        private readonly KscHrContext _kscHrContext;
        public AnalysisEmployeeAttendAbsenceItemRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<AnalysisEmployeeAttendAbsenceItem> GetEmployeeAttendAbsenceItemByEmployee(int employeeId, int workCalendarId)
        {
            var query = _kscHrContext.AnalysisEmployeeAttendAbsenceItems.Where(x => x.EmployeeId == employeeId && x.WorkCalendarId == workCalendarId);
            return query;
        }
        public void RemoveRangeAnalysisEmployeeAttendAbsenceItem(List<AnalysisEmployeeAttendAbsenceItem> model)
        {
            _kscHrContext.AnalysisEmployeeAttendAbsenceItems.RemoveRange(model);
        }
        public async void AddRangeAnalysisEmployeeAttendAbsenceItem(List<AnalysisEmployeeAttendAbsenceItem> model)
        {
            await _kscHrContext.AnalysisEmployeeAttendAbsenceItems.AddRangeAsync(model);
        }

    }
}

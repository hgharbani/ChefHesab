using KSC.Domain;
using Ksc.HR.Domain.Entities.Personal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ksc.HR.Domain.Repositories.Personal
{
    public interface IAnalysisEmployeeAttendAbsenceItemRepository : IRepository<AnalysisEmployeeAttendAbsenceItem, long>
    {
        IQueryable<AnalysisEmployeeAttendAbsenceItem> GetEmployeeAttendAbsenceItemByEmployee(int employeeId, int workCalendarId);
        void RemoveRangeAnalysisEmployeeAttendAbsenceItem(List<AnalysisEmployeeAttendAbsenceItem> model);
    }
}

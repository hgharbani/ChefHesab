using KSC.Domain;
using Ksc.HR.Domain.Entities.Emp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Share.Model.EmployeeConditionalAbsence;

namespace Ksc.HR.Domain.Repositories.Emp
{
    public interface IEmployeeConditionalAbsenceRepository : IRepository<EmployeeConditionalAbsence, int>
    {
        IQueryable<EmployeeConditionalAbsence> GetAllByDeletedRelated();
        IQueryable<EmployeeConditionalAbsence> GetAllByEmployeeId(int employeeId);
        IQueryable<EmployeeConditionalAbsence> GetAllRelated();
        IQueryable<EmployeeConditionalAbsence> GetEmployeeConditionalAbcenseNotHaveForcedOvertime(int yearMonth);
        EmployeeConditionalAbsence GetEmployeeConditionalAbsenceForCreateItemAttendAbsence(int employeeId, DateTime date, int conditionalAbsenceSubjectTypeId);
        EmployeeConditionalAbsenceForTimeSheetAnalysModel GetEmployeeConditionalAbsenceForTimeSheetAnalys(int employeeId, DateTime date);
        IQueryable<EmployeeConditionalAbsence> GetEmployeeConditionalAbsenceInDuration(int employeeId,DateTime startDate, DateTime endDate);
    }
}

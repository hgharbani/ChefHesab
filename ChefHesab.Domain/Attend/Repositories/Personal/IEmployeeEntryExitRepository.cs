using KSC.Domain;
using Ksc.HR.Domain.Entities.Personal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.Personal
{
    public interface IEmployeeEntryExitRepository : IRepository<EmployeeEntryExit, long>
    {
        void DisposeOperation();
        IQueryable<EmployeeEntryExit> GetEmployeeEntryExitActiveByDate(DateTime date);
        IQueryable<EmployeeEntryExit> GetEmployeeEntryExitByDate(int employeeId, DateTime entryExitDate);
        IQueryable<EmployeeEntryExit> GetEmployeeEntryExitByDate(DateTime entryExitDate);
        IEnumerable<EmployeeEntryExit> GetEmployeeEntryExitValidByDate(string person, DateTime entryExitDate);
        IQueryable<EmployeeEntryExit> GetEmployeeEntryExitValidByDate(DateTime date);
        IQueryable<EmployeeEntryExit> GetEmployeeEntryExitValidByEmployeeIdDate(int employeeId, DateTime entryExitDate);
        IQueryable<EmployeeEntryExit> GetEmployeeEntryExitValidByEmployeeIdRangeDate(int employeeId, DateTime startDate, DateTime endDate);
        IQueryable<EmployeeEntryExit> GetEmployeeEntryExitValidByEmployeeIdRangeDateForReport(int employeeId, DateTime startDate, DateTime endDate);
        IQueryable<EmployeeEntryExit> GetEmployeeEntryExitValidByEmployeeIdsRangeDate(List<int> employeeId, DateTime entryExitDate);
        IQueryable<EmployeeEntryExit> GetEmployeeEntryExitValidByEmployeeIdsRangeDateForReport(List<int> employeeId, DateTime startDate, DateTime endDate);
        IQueryable<EmployeeEntryExit> GetEmployeeEntryExitValidByEmployeeIdsRangeDateForReportWithEmployeeeId(int employeeId, DateTime startDate, DateTime endDate);

    }


}

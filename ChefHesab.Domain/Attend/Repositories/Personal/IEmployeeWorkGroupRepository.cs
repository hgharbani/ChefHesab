using KSC.Domain;
using Ksc.HR.Domain.Entities.Personal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.Personal
{
    public interface IEmployeeWorkGroupRepository : IRepository<EmployeeWorkGroup, int>
    {
        EmployeeWorkGroup GetActiveWorkGroupByEmployeeId(int EmployeeId);
        Task<EmployeeWorkGroup> GetActiveWorkGroupByEmployeeIdAsync(int EmployeeId);
        Task<EmployeeWorkGroup> GetEmployeeWorkGroupByEmployeeIdDate(int employeeId, DateTime date);
        IQueryable<EmployeeWorkGroup> GetEmployeeWorkGroupInclouded();

        IEnumerable<(EmployeeWorkGroup employeeWorkGroup, int? EmployeeCityId,int? WorkTimeId,string WorkGroupCode, bool ShiftSettingFromShiftboard, bool OfficialUnOfficialHolidayFromWorkCalendar)>
            GetEmployeeWorkGroupWithEmployeeCityId();
        IQueryable<EmployeeWorkGroup> GetEmployeeWorkGroupIncloudedWorkTimeWorkGroup();
        IEnumerable<(EmployeeWorkGroup employeeWorkGroup, int? EmployeeCityId, int? WorkTimeId, string WorkGroupCode, bool ShiftSettingFromShiftboard, bool OfficialUnOfficialHolidayFromWorkCalendar)> GetEmployeeWorkGroupByDateWithEmployeeCityId(DateTime date);
        Task<EmployeeWorkGroup> GetEmployeeWorkGroupByEmployeeIdDateIncludeWorkGroup(int employeeId, DateTime date);
        Task<EmployeeWorkGroup> GetEmployeeWorkGroupByEmployeeIdDateOutIncluded(int employeeId, DateTime date);
        EmployeeWorkGroup GetEmployeeWorkGroupByEmployeeIdDateIncludeByWorkGroup(int employeeId, DateTime date);
        IQueryable<EmployeeWorkGroup> GetEmployeeWorkGroupIncloudWorkGroup();
        IQueryable<EmployeeWorkGroup> GetEmployeeWorkGroupIncludeTransferRequest();
        EmployeeWorkGroup GetLastDeActiveWorkGroupByEmployeeId(int EmployeeId,DateTime endDate);
        IQueryable<EmployeeWorkGroup> GetDeActiveWorkGroupByEmployeeId(int EmployeeId);
        IQueryable<EmployeeWorkGroup> GetEmployeeWorkGroupByEmployeeId(int employeeId);
        EmployeeWorkGroup GetEmployeeWorkGroupByEmployeeIdDate(DateTime date, List<EmployeeWorkGroup> model);
    }
}


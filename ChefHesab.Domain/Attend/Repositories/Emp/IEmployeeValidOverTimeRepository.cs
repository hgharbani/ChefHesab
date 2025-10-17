using Ksc.HR.Domain.Entities;
using Ksc.HR.Domain.Entities.Emp;
using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.Emp
{
    public interface IEmployeeValidOverTimeRepository : IRepository<EmployeeValidOverTime, long>
    {
        IQueryable<EmployeeValidOverTime> GetAllQueryable();
        IQueryable<EmployeeValidOverTime> GetEmployeeValidOverTimeByWorkCalendarId(int workCalendarId);
        IQueryable<EmployeeValidOverTime> GetEmployeeValidOverTimeByEmployeeId(int employeeId);
        bool ValidOverTimeByEmployeeId(int employeeId, int workCalnedarId);
    }
}

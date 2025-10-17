using KSC.Domain;
using Ksc.HR.Domain.Entities.Personal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.Personal
{
    public interface IEmployeeEducationTimeRepository : IRepository<EmployeeEducationTime, int>
    {
        IQueryable<EmployeeEducationTime> GetActiveEmployeeEducationTimeByEmployeeIdWorkCalendar(int employeeId,int workCalendarId);
    }


}

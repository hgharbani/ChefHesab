using KSC.Domain;
using Ksc.HR.Domain.Entities.Workshift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.WorkShift
{
    public interface IWorkTimeRepository : IRepository<WorkTime, int>
    {
        IEnumerable<WorkTime> GetWorkTimes();
        WorkTime GetWorkTimeByCode(string code);
        WorkTime GetWorkTimeByIdAsNoTracking(int id);
        WorkTime GetWorkTimeInAttendAbsenceItemAsNoTracking(int employeeId, int workCalendarId);
        IQueryable<WorkTime> GetWorkTimesAsNoTracking();
        IEnumerable<WorkTime> GetAllActive();
    }
}

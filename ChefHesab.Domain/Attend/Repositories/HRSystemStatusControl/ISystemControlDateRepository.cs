using KSC.Domain;
using Ksc.HR.Domain.Entities.HRSystemStatusControl;
using Ksc.HR.Domain.Entities.Oncall;
using Ksc.HR.Domain.Entities.Workshift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.HRSystemStatusControl
{
    public interface ISystemControlDateRepository : IRepository<SystemControlDate, int>
    {
        SystemControlDate GetActiveData();
        int GetSalaryYear();
        int GetSalaryYear(SystemControlDate model);
        SystemControlDate InsertMonthToAttendAbsenceItemDate();
    }
}

using KSC.Domain;
using Ksc.HR.Domain.Entities.HRSystemStatusControl;
using Ksc.HR.Domain.Entities.Oncall;
using Ksc.HR.Domain.Entities.Workshift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Domain.Entities.BusinessTrip;
using Ksc.HR.Domain.Entities.EmployeeBase;

namespace Ksc.HR.Domain.Repositories.EmployeeBase
{
    public interface IMilitaryStatusRepository : IRepository<MilitaryStatus, int>
    {
        IQueryable<MilitaryStatus> GetAllMilitaryStatusNoTracking(int id);
        IQueryable<MilitaryStatus> GetDataFromMilitaryStatusForKSCContract();
    }
}

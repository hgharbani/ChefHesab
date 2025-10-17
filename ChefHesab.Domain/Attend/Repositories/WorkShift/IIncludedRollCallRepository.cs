using KSC.Domain;
using Ksc.HR.Domain.Entities.Workshift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.WorkShift
{
    public interface IIncludedRollCallRepository : IRepository<IncludedRollCall, int>
    {
        IQueryable<IncludedRollCall> GetAllByRelated();
        IQueryable<IncludedRollCall> GetAllAsNoTrackingByIncludedDefinition();
        IQueryable<IncludedRollCall> GetActiveIncludedRollCallByIncludedIdAsNoTracking(int includedDefinitionId);
        List<int> GetRollCallDefinitionByIncludedDefinitionCode(string code);
    }

}

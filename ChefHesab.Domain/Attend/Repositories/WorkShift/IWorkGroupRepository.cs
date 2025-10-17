using KSC.Domain;
using Ksc.HR.Domain.Entities.Workshift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.WorkShift
{
    public interface IWorkGroupRepository : IRepository<WorkGroup, int>
    {
        Task<WorkGroup> GetWorkGroupByRelations(int id);
        Task<WorkGroup> GetWorkGroupIncludWorkTime(int id);
        IQueryable<WorkGroup> GetWorkGroupsByRelations(List<int> id);
        Task<WorkGroup> GetWorkGroupsByWorkTimeRelations(int id);
        WorkGroup GetWorkGroupsByWorkTimeRelations_N(int id);
        WorkGroup GetWorkGroupsByWorkTimeIdAndCode(int workTimeId, string code);
        Task<WorkGroup> GetWorkGroupOutInclud(int id);
        IQueryable<WorkGroup> GetWorkGroupRelatiedWorkTime();
        IQueryable<WorkGroup> GetDataFromWorkGroupForKSCContract();
    }
}

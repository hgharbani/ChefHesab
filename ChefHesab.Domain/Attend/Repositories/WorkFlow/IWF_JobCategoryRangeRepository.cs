using KSC.Domain;
using Ksc.HR.Domain.Entities.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.WorkFlow
{
    public interface IWF_JobCategoryRangeRepository : IRepository<WF_JobCategoryRange, int>
    {
        WF_JobCategoryRange GetWFJobCategoryRangeByProcessId(int processId);
        IQueryable<WF_JobCategoryRange> GetWFJobCategoryRangeIncludeActiveValidJobCategories();
    }
}

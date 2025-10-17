using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.WorkFlow;
using Ksc.HR.Domain.Repositories.WorkFlow;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.WorkFlow
{
    public class WF_JobCategoryRangeRepository : EfRepository<WF_JobCategoryRange, int>, IWF_JobCategoryRangeRepository
    {
        private readonly KscHrContext _kscHrContext;
        public WF_JobCategoryRangeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public WF_JobCategoryRange GetWFJobCategoryRangeByProcessId(int processId)
        {
            return _kscHrContext.WF_JobCategoryRanges.AsQueryable().Include(x => x.WF_ValidJobCategories).Where(x => x.WorkFlowProcessId == processId).FirstOrDefault();
        }

        public IQueryable<WF_JobCategoryRange> GetWFJobCategoryRangeIncludeActiveValidJobCategories()
        {
            return _kscHrContext.WF_JobCategoryRanges.Include(x => x.WF_ValidJobCategories)
                .Where(x => x.WF_ValidJobCategories.Any(x => x.IsActive));
        }

    }
}

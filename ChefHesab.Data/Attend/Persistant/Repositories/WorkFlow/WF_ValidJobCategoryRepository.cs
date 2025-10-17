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
    public class WF_ValidJobCategoryRepository : EfRepository<WF_ValidJobCategory, int>, IWF_ValidJobCategoryRepository
    {
        private readonly KscHrContext _kscHrContext;
        public WF_ValidJobCategoryRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext= KscHrContext;
        }
        public IQueryable<WF_ValidJobCategory> GetWF_ValidJobCategoryByProcessId(int processId)
        {
            return _kscHrContext.WF_ValidJobCategories.Include(x => x.WF_JobCategoryRange).ThenInclude(x=>x.WF_Process).Where(x => x.WF_JobCategoryRange.WorkFlowProcessId == processId);
        }


    }
}

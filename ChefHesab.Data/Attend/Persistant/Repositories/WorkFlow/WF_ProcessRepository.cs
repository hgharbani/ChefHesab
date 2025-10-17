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
    public class WF_ProcessRepository : EfRepository<WF_Process, int>, IWF_ProcessRepository
    {
        private readonly KscHrContext _kscHrContext;
        public WF_ProcessRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext= KscHrContext;
        }
        public IQueryable<WF_Process> GetProcessByParent (int parentId)
        {
            return _kscHrContext.WF_Processes.Where(x => x.ParentProcessId == parentId && x.IsActive);
        }
        public IQueryable<WF_Process> GetProcessIncludeParent()
        {
            return _kscHrContext.WF_Processes.Where(x =>x.IsActive).Include(x=>x.ParentProcess);
        } 
        public IQueryable<WF_Process> GetAllProcess()
        {
            return _kscHrContext.WF_Processes.Where(x =>x.IsActive);
        }

    }
}

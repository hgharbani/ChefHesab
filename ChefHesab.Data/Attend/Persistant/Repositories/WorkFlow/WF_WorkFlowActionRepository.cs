using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.WorkFlow;
using Ksc.HR.Domain.Repositories.WorkFlow;
using Microsoft.EntityFrameworkCore;

namespace Ksc.HR.Data.Persistant.Repositories.WorkFlow
{
    public class WF_WorkFlowActionRepository : EfRepository<WF_WorkFlowAction, int>, IWF_WorkFlowActionRepository
    {
        private readonly KscHrContext _kscHrContext;
        public WF_WorkFlowActionRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public WF_WorkFlowAction GetWorkFlowActionIncludedProcess(int id)
        {
            return _kscHrContext.WF_WorkFlowActions.Include(x => x.WF_Process).ThenInclude(x => x.WF_Processes).FirstOrDefault(x => x.Id == id);
        }
    }
}

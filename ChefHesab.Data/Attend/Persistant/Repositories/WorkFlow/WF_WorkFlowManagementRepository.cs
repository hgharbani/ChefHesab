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
    public class WF_WorkFlowManagementRepository : EfRepository<WF_WorkFlowManagement, int>, IWF_WorkFlowManagementRepository
    {
        private readonly KscHrContext _kscHrContext;
        public WF_WorkFlowManagementRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public WF_WorkFlowManagement GetWorkFlowManagementByCurrentStatusAndnextStatus(int processId, int currentStatusId, int nextStatusId)
        {
            return _kscHrContext.WF_WorkFlowManagements.FirstOrDefault(x =>x.IsActive && x.ProcessId == processId && x.CurrentStatusId == currentStatusId && x.NextStatusId == nextStatusId);
        }
        public WF_WorkFlowManagement GetWorkFlowManagementAutoByCurrentStatus(int processId, int currentStatusId)
        {
            return _kscHrContext.WF_WorkFlowManagements.FirstOrDefault(x => x.IsActive && x.IsManual == false && x.ProcessId == processId && x.CurrentStatusId == currentStatusId);
        }
        public WF_WorkFlowManagement GetWorkFlowManagementByPriorityId(int processId, int priorityId, int currentStatusId)
        {
            return _kscHrContext.WF_WorkFlowManagements.FirstOrDefault(x => x.IsActive && x.IsManual == false && x.ProcessId == processId && x.PriorityId == priorityId && x.CurrentStatusId == currentStatusId);
        }
        public IQueryable<WF_WorkFlowManagement> GetNextWorkFlowManagementByCurrentStatus(int processId, int currentStatusId)
        {
            return _kscHrContext.WF_WorkFlowManagements.Where(x => x.ProcessId == processId && x.CurrentStatusId == currentStatusId && x.IsActive);
        }
        public IQueryable<WF_WorkFlowManagement> GetWorkFlowManagementActive()
        {
            return _kscHrContext.WF_WorkFlowManagements.Where(x => x.IsActive);
        }

    }
}

using KSC.Domain;
using Ksc.HR.Domain.Entities.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.WorkFlow
{
    public interface IWF_StatusProcessManagementRepository : IRepository<WF_StatusProcessManagement, int>
    {
        IQueryable<WF_StatusProcessManagement> GetActiveStatusProcessManagementIncludedStatus_ProcessSync();
        IQueryable<WF_StatusProcessManagement> GetActiveWorkFlowStatusProcessManagementAsNoTracking();
        WF_StatusProcessManagement GetActiveWorkFlowStatusProcessManagementByProcessIdAndStatusId(int ProcessId, int StatusId);
        Task<WF_StatusProcessManagement> GetActiveWorkFlowStatusProcessManagementByProcessIdAndStatusIdSync(int ProcessId, int StatusId);
        IQueryable<WF_StatusProcessManagement> GetActiveWorkFlowStatusProcessManagementSyncAsNotracking();
        IQueryable<WF_StatusProcessManagement> GetStatusProcessManagementActive();

        // Tuple<IEnumerable<WF_StatusProcessManagement>, IEnumerable<WF_AccessManagement>> GetKartablAccesses(IEnumerable<WF_AccessManagement> accessManagements, bool isRequest, bool isInbox, bool? isResponser = false);
        IEnumerable<WF_StatusProcessManagement> GetStatusProcessManagementForKartablAccesses(bool isRequest, bool isInbox, bool? isResponser = false);
        IQueryable<WF_StatusProcessManagement> GetStatusProcessManagementForOutBox();
        Task<WF_StatusProcessManagement> GetStatusProcessManagementValidShowInISKartabl(int statusId, int processId);
    }
}

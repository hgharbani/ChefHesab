using KSC.Domain;
using Ksc.HR.Domain.Entities.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.WorkFlow
{
    public interface IWF_WorkFlowManagementRepository : IRepository<WF_WorkFlowManagement, int>
    {
        IQueryable<WF_WorkFlowManagement> GetNextWorkFlowManagementByCurrentStatus(int processId, int currentStatusId);
        IQueryable<WF_WorkFlowManagement> GetWorkFlowManagementActive();
        WF_WorkFlowManagement GetWorkFlowManagementAutoByCurrentStatus(int processId, int currentStatusId);
        WF_WorkFlowManagement GetWorkFlowManagementByCurrentStatusAndnextStatus(int processId, int currentStatusId, int nextStatusId);
        WF_WorkFlowManagement GetWorkFlowManagementByPriorityId(int processId, int priorityId,int nextStatusId);
    }
}

using KSC.Domain;
using Ksc.HR.Domain.Entities.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.WorkFlow
{
    public interface IWF_AccessManagementRepository : IRepository<WF_AccessManagement, int>
    {
        public IEnumerable<WF_AccessManagement> GetAllByPersonId(string personId, string jopPositionCode);
        public IEnumerable<WF_AccessManagement> GetAccessListforResponser(string personId, string jopPositionCode);
        IQueryable<WF_AccessManagement> GetValidWorkFlowAccessManagement();
        IEnumerable<WF_AccessManagement> GetAllByPersonIdProcessId(string personId, int processId);
        WF_AccessManagement GetWF_AccessManagementByProcessIdRoleId(int processId, int roleId);
        IQueryable<WF_AccessManagement> GetWF_AccessManagementByWinUser(string winUser);
        IQueryable<WF_AccessManagement> GetAllByPersonalId(string personId, string jopPositionCode);
        IQueryable<WF_AccessManagement> GetDataAccessListforResponser(string personId, string jopPositionCode);
    }
}

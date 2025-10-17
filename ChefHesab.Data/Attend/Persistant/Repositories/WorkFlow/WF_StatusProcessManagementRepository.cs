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
    public class WF_StatusProcessManagementRepository : EfRepository<WF_StatusProcessManagement, int>, IWF_StatusProcessManagementRepository
    {
        private readonly KscHrContext _kscHrContext;
        public WF_StatusProcessManagementRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        //public Tuple<IEnumerable<WF_StatusProcessManagement>, IEnumerable<WF_AccessManagement>> GetKartablAccesses(IEnumerable<WF_AccessManagement> accessManagements, bool isRequest, bool isInbox, bool? isResponser = false)
        //{

        //    var result = GetAll().Where(c => c.IsRequest == isRequest && c.IsInbox == isInbox && (c.IsReferToShow && isResponser == true || isResponser == false)).Join(accessManagements,
        //      a => new { key1 = a.RoleId, key2 = a.ProcessId },
        //      b => new { key1 = b.RoleId, key2 = b.ProcessId },
        //      (a, b) => new { statusProcessManagementResult = a, accessManagementresult = b });

        //    return new Tuple<IEnumerable<WF_StatusProcessManagement>, IEnumerable<WF_AccessManagement>>(result.Select(x=>x.statusProcessManagementResult),result.Select(x=>x.accessManagementresult));
        //}
        public IEnumerable<WF_StatusProcessManagement> GetStatusProcessManagementForKartablAccesses(bool isRequest, bool isInbox, bool? isResponser = false)
        {
            //return GetAll().Where(c => c.IsRequest == isRequest && c.IsInbox == isInbox && ((c.IsReferToShow && isResponser == true) || isResponser == false));
            return _kscHrContext.WF_StatusProcessManagements.Where(c => c.IsRequest == isRequest && c.IsInbox == isInbox && ((c.IsReferToShow && isResponser == true) || isResponser == false));
        }
        public WF_StatusProcessManagement GetActiveWorkFlowStatusProcessManagementByProcessIdAndStatusId(int ProcessId, int StatusId)
        {
            return _kscHrContext.WF_StatusProcessManagements.Where(x => x.IsActive && x.ProcessId == ProcessId && x.StatusId == StatusId).FirstOrDefault();
        }

        public async Task<WF_StatusProcessManagement> GetActiveWorkFlowStatusProcessManagementByProcessIdAndStatusIdSync(int ProcessId, int StatusId)
        {
            return await _kscHrContext.WF_StatusProcessManagements.Include(x => x.WF_Role).FirstOrDefaultAsync(x => x.IsActive && x.ProcessId == ProcessId && x.StatusId == StatusId);
        }
        public IQueryable<WF_StatusProcessManagement> GetActiveWorkFlowStatusProcessManagementSyncAsNotracking()
        {
            return _kscHrContext.WF_StatusProcessManagements.Include(x => x.WF_Role).Where(x => x.IsActive).AsTracking();
        }
        public IQueryable<WF_StatusProcessManagement> GetActiveStatusProcessManagementIncludedStatus_ProcessSync() => _kscHrContext.WF_StatusProcessManagements.Include(x => x.WF_Status).Include(x => x.WF_Process).Where(x => x.IsActive && x.WF_Status.IsActive).AsQueryable().AsNoTracking();
        public async Task<WF_StatusProcessManagement> GetStatusProcessManagementValidShowInISKartabl(int statusId, int processId)
        {
            return await _kscHrContext.WF_StatusProcessManagements.FirstOrDefaultAsync(x => x.IsActive && x.StatusId == statusId && x.ProcessId == processId && x.ShowInISKartabl);
        }
        public IQueryable<WF_StatusProcessManagement> GetActiveWorkFlowStatusProcessManagementAsNoTracking() => _kscHrContext.WF_StatusProcessManagements.Where(x => x.IsActive).AsNoTracking();
        public IQueryable<WF_StatusProcessManagement> GetStatusProcessManagementForOutBox()
        {
            return _kscHrContext.WF_StatusProcessManagements.Where(x => x.IsActive && x.IsPublicKartabl && x.IsNotInbox);
        }
        public IQueryable<WF_StatusProcessManagement> GetStatusProcessManagementActive()
        {
            return _kscHrContext.WF_StatusProcessManagements.Where(x => x.IsActive);
        }
    }
}

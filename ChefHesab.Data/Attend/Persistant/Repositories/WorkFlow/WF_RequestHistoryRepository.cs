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
    public class WF_RequestHistoryRepository : EfRepository<WF_RequestHistory, int>, IWF_RequestHistoryRepository
    {
        private readonly KscHrContext _kscHrContext;
        public WF_RequestHistoryRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        // find parentId in reqestHistory
        #region  

        public int? FindParentIdInRequestHistory(int requestId)
        {
            var result = _kscHrContext.WF_RequestHistories.Where(x => x.RequestId == requestId).OrderByDescending(x => x.Id).FirstOrDefault();
            return result != null ? result.Id : null;
        }
        public IEnumerable<WF_RequestHistory> GetIncludedRequestHistory()
        {
            var result = _kscHrContext.WF_RequestHistories.Include(a => a.WF_RequestHistories);
            return result;
        }
        public IQueryable<WF_RequestHistory> GetIncludedRequest()
        {
            var result = _kscHrContext.WF_RequestHistories.Include(a => a.WF_Request).Include(a => a.WF_Status).AsNoTracking();
            return result;
        }
        public IQueryable<WF_RequestHistory> GetIncludedRequestAction()
        {
            var result = _kscHrContext.WF_RequestHistories.Include(a => a.WF_Request).Include(a => a.WF_Status).Include(x=>x.WF_WorkFlowAction).AsNoTracking();
            return result;
        }
        public IQueryable<WF_RequestHistory> GetRequestHistoryByRequestId(int requestId)
        {
            var result = _kscHrContext.WF_RequestHistories.Where(x => x.RequestId == requestId);
            return result;
        }


        public void UpdateRange(List<WF_RequestHistory> wfRequestHistory)
        {
            _kscHrContext.WF_RequestHistories.UpdateRange(wfRequestHistory);
        }
        #endregion

    }
}

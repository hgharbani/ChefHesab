using KSC.Domain;
using Ksc.HR.Domain.Entities.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.WorkFlow
{
    public interface IWF_RequestHistoryRepository : IRepository<WF_RequestHistory, int>
    {


        int? FindParentIdInRequestHistory(int requestId);
        IQueryable<WF_RequestHistory> GetIncludedRequest();
        IQueryable<WF_RequestHistory> GetIncludedRequestAction();
        IQueryable<WF_RequestHistory> GetRequestHistoryByRequestId(int requestId);
        void UpdateRange(List<WF_RequestHistory> wfRequestHistory);
    }
}

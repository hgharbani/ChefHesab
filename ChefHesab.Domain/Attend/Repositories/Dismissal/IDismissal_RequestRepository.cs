using Ksc.HR.Domain.Entities.Dismissal;
using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.Dismissal
{
    public interface IDismissal_RequestRepository : IRepository<Dismissal_Request, int>
    {
        IEnumerable<Dismissal_Request> GetDismissalRequestByRealtedHistoriesEntities();
        IEnumerable<Dismissal_Request> GetDismissalRequestByAllRealtedEntities();
        IEnumerable<Dismissal_Request> GetDismissalRequestByRealtedEntities();
        IEnumerable<Dismissal_Request> GetDismissalRequestEmployeeByRealtedEntities();
        Dismissal_Request GetDismissalRequestByWFRequestId(int requestId);
        Dismissal_Request GetDismissalRequestByWFRequestIdIncluded(int requestId);
    }
}

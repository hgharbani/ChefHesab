using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Dismissal;
using Ksc.HR.Domain.Repositories.Dismissal;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.Dismissal
{
    public class Dismissal_RequestRepository : EfRepository<Dismissal_Request, int>, IDismissal_RequestRepository
    {
        private readonly KscHrContext _kscHrContext;
        public Dismissal_RequestRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IEnumerable<Dismissal_Request> GetDismissalRequestByAllRealtedEntities()
        {
            return GetAll().AsQueryable().Include(x => x.WF_Request).ThenInclude(x=>x.WF_Requests).ThenInclude(x => x.WF_RequestHistories).Include(x=>x.Dismissal_Status).Include(x => x.WF_Request).ThenInclude(x=>x.WF_RequestHistories);
        }
        public IEnumerable<Dismissal_Request> GetDismissalRequestByRealtedEntities()
        {
            return GetAll().AsQueryable().Include(x => x.WF_Request).Include(x => x.Dismissal_Status).Include(x => x.WF_Request).ThenInclude(x => x.WF_RequestHistories);
        }
        public IEnumerable<Dismissal_Request> GetDismissalRequestByRealtedHistoriesEntities()
        {
            return GetAll().AsQueryable().Include(x => x.WF_Request).ThenInclude(x => x.WF_RequestHistories).ThenInclude(x => x.WF_Status).Include(x => x.Dismissal_Status).Include(x => x.WF_Request);
        }
        public IEnumerable<Dismissal_Request> GetDismissalRequestEmployeeByRealtedEntities()
        {
            return GetAll().AsQueryable().Include(x=>x.Dismissal_Status).Include(x => x.WF_Request).ThenInclude(x => x.Employee);
        }
        public Dismissal_Request GetDismissalRequestByWFRequestId(int requestId)
        {
            return GetAll().First(x => x.WfRequestId == requestId);
        }
        public Dismissal_Request GetDismissalRequestByWFRequestIdIncluded(int requestId)
        {
            return _kscHrContext.Dismissal_Request.Where(x => x.WfRequestId == requestId).Include(x => x.Dismissal_Status).Include(x=>x.WF_Request).ThenInclude(x=>x.Employee).FirstOrDefault();
        }
    }
}

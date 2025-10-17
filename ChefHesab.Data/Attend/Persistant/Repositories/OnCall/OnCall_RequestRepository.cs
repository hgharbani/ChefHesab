using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Oncall;
using Ksc.HR.Domain.Repositories.OnCall;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.OnCall
{
    public class OnCall_RequestRepository : EfRepository<OnCall_Request, int>, IOnCall_RequestRepository
    {
        private readonly KscHrContext _kscHrContext;
        public OnCall_RequestRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public OnCall_Request GetRequestByRelated(int id)
        {
            return _kscHrContext.OnCall_Requests.Include(x => x.OnCall_Type).Include(a => a.WF_Request).ThenInclude(a => a.WF_RequestHistories).ThenInclude(a => a.WF_RequestHistories).First(x => x.Id == id);
        }

        public OnCall_Request GetRequestByRequestId(int reqId)
        {
            return _kscHrContext.OnCall_Requests.Include(x => x.OnCall_Type)
                .Include(a => a.WF_Request)
                .Include(x => x.WF_Request.WF_RequestHistories).ThenInclude(a => a.WF_RequestHistories)
                .Include(a => a.WF_Request.Employee)

                .FirstOrDefault(x => x.RequestId == reqId);
        }
        public IQueryable<OnCall_Request> GetOnCallRequestByOncallDate(DateTime statrtDate, DateTime endDate)
        {
            return _kscHrContext.OnCall_Requests.Include(x => x.OnCall_Type).Where(x => x.OnCallDate.Date >= statrtDate && x.OnCallDate.Date <= endDate.Date);
        }
        public OnCall_Request GetRequestByRequestIdNoIncluded(int reqId)
        {
            return _kscHrContext.OnCall_Requests.FirstOrDefault(x => x.RequestId == reqId);
        }
        public IQueryable<OnCall_Request> GetRequestByEmployeeIdOncallDate(int employeeId, DateTime date, int invalidStatusId)
        {
            return _kscHrContext.OnCall_Requests.Include(x => x.WF_Request).Where(x => x.WF_Request.EmployeeId == employeeId && x.OnCallDate == date && x.WF_Request.StatusId != invalidStatusId);
        }
        public IQueryable<OnCall_Request> GetOnCallRequestInCludeWfRequestByOncallDate(DateTime statrtDate, DateTime endDate,int statusId)
        {
            return _kscHrContext.OnCall_Requests.Include(x => x.WF_Request).ThenInclude(x=>x.Employee).ThenInclude(x=>x.TeamWork).Where(x => x.OnCallDate.Date >= statrtDate && x.OnCallDate.Date <= endDate.Date && x.WF_Request.StatusId==statusId);
        }
    }
}

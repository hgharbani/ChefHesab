using KSC.Domain;
using Ksc.HR.Domain.Entities.Oncall;
using Ksc.HR.Domain.Entities.Workshift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.OnCall
{
    public interface IOnCall_RequestRepository : IRepository<OnCall_Request, int>
    {
        IQueryable<OnCall_Request> GetOnCallRequestByOncallDate(DateTime statrtDate, DateTime endDate);
        IQueryable<OnCall_Request> GetOnCallRequestInCludeWfRequestByOncallDate(DateTime statrtDate, DateTime endDate,int statusId);
        IQueryable<OnCall_Request> GetRequestByEmployeeIdOncallDate(int employeeId, DateTime date, int invalidStatusId);
        OnCall_Request GetRequestByRelated(int id);
        OnCall_Request GetRequestByRequestId(int reqId);
        OnCall_Request GetRequestByRequestIdNoIncluded(int reqId);
    }
}

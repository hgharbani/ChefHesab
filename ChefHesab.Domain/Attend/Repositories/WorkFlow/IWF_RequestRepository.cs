using KSC.Domain;
using Ksc.HR.Domain.Entities.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Share.Model.WF_Request;
using KSC.Common;
using Ksc.HR.Share.Model.Rule;

namespace Ksc.HR.Domain.Repositories.WorkFlow
{
    public interface IWF_RequestRepository : IRepository<WF_Request, int>
    {
        IQueryable<WF_Request> GetAllRequestByAllRelated();
       // IQueryable<WF_Request> GetAllRequestByEmployeeIdProcessId(int employeeId, int processId);
        Task<WF_Request> GetAllRequestByEmployeeIdProcessId(int employeeId, int processId);
        IEnumerable<WF_Request> GetAllRequestByRelated();
        IEnumerable<WF_Request> GetRequestsIncludEmployee();
        Task<WF_Request> GetRequestIncludChildRequestAsNotracking(int id);
        Task<WF_Request> GetRequestIncludHistoryIncludChildRequest(int id);
        IQueryable<WF_Request> GetAllRequestForOutBox();
        IEnumerable<WF_Request> GetRequestsIncludStatusEmployee();
        IQueryable<WF_Request> GetActiveRequest();
        IQueryable<WF_Request> GetAllRequestByRelatedEntity();
        IQueryable<WF_Request> GetActiveRequestsIncludStatusEmployee();
        Task<WF_Request> GetAllRequestById(int id);
        //Task<KscResult> PromotionConfirmInWorkFlowRequest(ChangeGroupStatusInRequestEndModel model);
        Task ChangeWorkFlowRequestStatus(ChangeGroupStatusInRequestEndModel model);
        void UpdateRange(List<WF_Request> wfRequest);
        Task<PromotionConfirmHelper> PromotionConfirmInWorkFlowRequest(ChangeGroupStatusInRequestEndModel model);
    }

    public class PromotionConfirmHelper
    {
        public PromotionConfirmHelper()
        {
            OldHistories = new List<WF_RequestHistory>();
            NewHistories = new List<WF_RequestHistory>();
            NewRequests = new List<WF_Request>();
        }
        public List<WF_RequestHistory> OldHistories { get; set; }
        public List<WF_RequestHistory> NewHistories { get; set; }
        public List<WF_Request> NewRequests { get; set; }
        public KscResult Result { get; set; }

    }

}

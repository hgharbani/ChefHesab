using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities;
using Ksc.HR.Domain.Entities.Transfer;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Repositories.WorkFlow;
using Ksc.HR.Domain.Repositories.WorkShift;
using Microsoft.EntityFrameworkCore;

namespace Ksc.HR.Data.Persistant.Repositories.Transfer
{
    public partial class Transfer_RequestRepository : EfRepository<Transfer_Request, int>, ITransfer_RequestRepository
    {
        private readonly KscHrContext _kscHrContext;
        public Transfer_RequestRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public void DeleteById(int id)
        {
            var transfer_Request = GetById(id);
            var wf_Request = _kscHrContext.WF_Requests.AsQueryable().Include(x => x.WF_RequestHistories)
                                                        .FirstOrDefault(x => x.Id == transfer_Request.WfRequestId);
            _kscHrContext.Transfer_Requests.Remove(transfer_Request);
            _kscHrContext.WF_RequestHistories.RemoveRange(wf_Request.WF_RequestHistories);
            _kscHrContext.WF_Requests.Remove(wf_Request);
        }

        public Transfer_Request GetByIdIncludeWF_Req(int id)
        {
            var result = _kscHrContext.Transfer_Requests.AsQueryable().Include(x => x.WF_Request).FirstOrDefault(x => x.Id == id);
            return result;

        }

        public IQueryable<Transfer_Request> GetTransferRequestByRelated()
        {
            return GetAll().AsQueryable().Include(x => x.WF_Request).Include(x => x.Transfer_RequestReasonType).ThenInclude(x => x.Transfer_Type)
                .Include(x => x.RequestdTeamWork).Include(x => x.RequestdWorkGroup)
                .Include(x => x.LastWorkGroup).Include(x => x.LastTeamWork)
                ;
        }
        public Transfer_Request GetTransferRequestByWFRequestId(int WfRequestId)
        {
            return GetAll().AsQueryable().Include(x => x.WF_Request).ThenInclude(x => x.Employee)
                .Include(x => x.Transfer_RequestReasonType).ThenInclude(x => x.Transfer_Type)
                .Include(x => x.Transfer_RequestReasonType).ThenInclude(x => x.Transfer_RequestReason)
                .Include(x => x.LastWorkGroup).ThenInclude(x => x.WorkTime).Include(x => x.LastTeamWork)
                .Include(x => x.Transfer_RequestReasonType).ThenInclude(x => x.Transfer_RequestType)
                .Include(x => x.RequestdTeamWork).Include(x => x.RequestdWorkGroup).ThenInclude(x => x.WorkTime).FirstOrDefault(x => x.WfRequestId == WfRequestId);
        }
        public IQueryable<Transfer_Request> GetTransferRequestByEemployeeIdRequestTypeId(int employeeId, int transferRequestTypeId)
        {
            return _kscHrContext.Transfer_Requests.Include(x => x.WF_Request).Include(x => x.Transfer_RequestReasonType).Include(x=>x.RequestdWorkGroup).Where(x => x.WF_Request.EmployeeId == employeeId
                && x.Transfer_RequestReasonType.TransferRequestTypeId == transferRequestTypeId);
           
        }


    }
}

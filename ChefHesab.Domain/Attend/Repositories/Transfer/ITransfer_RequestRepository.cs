using KSC.Domain;
using Ksc.HR.Domain.Entities.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.WorkFlow
{
    public interface ITransfer_RequestRepository : IRepository<Transfer_Request, int>
    {
        IQueryable<Transfer_Request> GetTransferRequestByRelated();
        Transfer_Request GetTransferRequestByWFRequestId(int WfRequestId);
        void DeleteById(int id);

        Transfer_Request GetByIdIncludeWF_Req(int id);
        IQueryable<Transfer_Request> GetTransferRequestByEemployeeIdRequestTypeId(int employeeId, int transferRequestTypeId);
    }
}

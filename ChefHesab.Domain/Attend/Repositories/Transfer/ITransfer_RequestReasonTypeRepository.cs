using KSC.Domain;
using Ksc.HR.Domain.Entities.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.WorkFlow
{
    public interface ITransfer_RequestReasonTypeRepository : IRepository<Transfer_RequestReasonType, int>
    {
        IQueryable<Transfer_RequestReasonType> GetRequestReasonTypeByCategoryCode(string categoryCode, int requestTypeId);
        Transfer_RequestReasonType GetRequestReasonTypeByRequestReasonTypeId(int requestReasonTypeId);
        IQueryable<Transfer_RequestReasonType> GetRequestReasonTypeByRequestTypeId(int requestTypeId);
    }
}

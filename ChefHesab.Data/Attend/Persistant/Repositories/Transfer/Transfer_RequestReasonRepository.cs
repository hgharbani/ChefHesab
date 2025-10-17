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
    public partial class Transfer_RequestReasonRepository : EfRepository<Transfer_RequestReason,int>, ITransfer_RequestReasonRepository
    {
        public Transfer_RequestReasonRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {

        }

     
    }
}

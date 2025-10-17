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
    public partial class Transfer_TypeRepository : EfRepository<Transfer_Type,int>, ITransfer_TypeRepository
    {
        public Transfer_TypeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {

        }

     
    }
}

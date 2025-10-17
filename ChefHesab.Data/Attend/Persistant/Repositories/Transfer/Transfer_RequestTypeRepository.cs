using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities;
using Ksc.HR.Domain.Entities.Transfer;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Repositories.WorkFlow;
using Ksc.HR.Domain.Repositories.WorkShift;
using Microsoft.EntityFrameworkCore;

namespace Ksc.HR.Data.Persistant.Repositories.WorkShift
{
    public partial class Transfer_RequestTypeRepository : EfRepository<Transfer_RequestType,int>, ITransfer_RequestTypeRepository
    {
        public Transfer_RequestTypeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {

        }

     
    }
}

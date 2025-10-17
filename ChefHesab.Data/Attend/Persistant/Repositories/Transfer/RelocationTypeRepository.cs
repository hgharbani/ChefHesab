using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities;
using Ksc.HR.Domain.Entities.Transfer;
using Ksc.HR.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Ksc.HR.Domain.Repositories.Transfer;

namespace Ksc.HR.Data.Persistant.Repositories.Transfer
{
    public partial class RelocationTypeRepository : EfRepository<RelocationType,int>, IRelocationTypeRepository
    {
        public RelocationTypeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {

        }

     
    }
}

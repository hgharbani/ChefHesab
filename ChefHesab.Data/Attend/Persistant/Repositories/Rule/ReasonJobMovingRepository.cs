using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Rule;
using Ksc.HR.Domain.Repositories.Rule;
using KSC.Infrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.Rule
{
    public class ReasonJobMovingRepository : EfRepository<ReasonJobMoving, int>, IReasonJobMovingRepository
    {
        private readonly KscHrContext _kscHrContext;
        public ReasonJobMovingRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
    }
}

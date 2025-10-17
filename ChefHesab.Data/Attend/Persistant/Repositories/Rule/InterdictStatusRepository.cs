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
    public class InterdictStatusRepository : EfRepository<InterdictStatus, int>, IInterdictStatusRepository
    {
        private readonly KscHrContext _kscHrContext;
        public InterdictStatusRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
    }
}

using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.HRSystemStatusControl;
using Ksc.HR.Domain.Entities.Oncall;
using Ksc.HR.Domain.Repositories.HRSystemStatusControl;
using Ksc.HR.Domain.Repositories.OnCall;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.HRSystemStatusControl
{
    public class SystemSequenceStatusRepository : EfRepository<SystemSequenceStatus, int>, ISystemSequenceStatusRepository
    {
        private readonly KscHrContext _kscHrContext;
        public SystemSequenceStatusRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

    }
}

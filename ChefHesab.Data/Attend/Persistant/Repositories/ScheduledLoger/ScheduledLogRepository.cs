using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Oncall;
using Ksc.HR.Domain.Repositories.OnCall;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Domain.Entities.ScheduledLoger;
using Ksc.HR.Domain.Repositories.ScheduledLoger;

namespace Ksc.HR.Data.Persistant.Repositories.ScheduledLoger
{
    public class ScheduledLogRepository : EfRepository<ScheduledLog, int>, IScheduledLogRepository
    {
        private readonly KscHrContext _kscHrContext;
        public ScheduledLogRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext= KscHrContext;
        }
      
    }
}

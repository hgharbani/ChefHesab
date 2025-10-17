using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Chart;
using Ksc.HR.Domain.Repositories.Chart;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Ksc.HR.Data.Persistant.Repositories.Chart
{
    public class JobGroupRepository : EfRepository<JobGroup, int>, IJobGroupRepository
    {
        private readonly KscHrContext _kscHrContext;
        public JobGroupRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<JobGroup> GetActiveQueryable()
        {
            return _kscHrContext.JobGroup.Where(a => a.IsActive == true);
        }
    }
}

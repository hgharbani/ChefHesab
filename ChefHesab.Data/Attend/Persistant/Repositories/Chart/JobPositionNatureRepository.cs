using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Chart;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Repositories.Chart;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Ksc.HR.Data.Persistant.Repositories.Chart
{
    public class JobPositionNatureRepository : EfRepository<Chart_JobPositionNature, int>, IJobPositionNatureRepository
    {
        private readonly KscHrContext _kscHrContext;
        public JobPositionNatureRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }


        public IQueryable<Chart_JobPositionNature> GetJobPositionNatureById(int id)
        {
            return _kscHrContext.Chart_JobPositionNature.Where(a => a.Id == id);
        }
        public IQueryable<Chart_JobPositionNature> GetJobPositionNatures()
        {
            var result = _kscHrContext.Chart_JobPositionNature.AsQueryable();
            return result;
        }

    }
}

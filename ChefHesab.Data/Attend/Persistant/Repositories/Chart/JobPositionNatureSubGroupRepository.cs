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
    public class JobPositionNatureSubGroupRepository : EfRepository<Chart_JobPositionNatureSubGroup, int>, IJobPositionNatureSubGroupRepository
    {
        private readonly KscHrContext _kscHrContext;
        public JobPositionNatureSubGroupRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }


        public IQueryable<Chart_JobPositionNatureSubGroup> GetSubGroupById(int id)
        {
            return _kscHrContext.JobPositionNatureSubGroups.Where(a => a.Id == id);
        }
        public IQueryable<Chart_JobPositionNatureSubGroup> GetSubGroups()
        {
            var result = _kscHrContext.JobPositionNatureSubGroups.AsQueryable();
            return result;
        }

        public IQueryable<Chart_JobPositionNatureSubGroup> GetSubGroupsRelated()
        {
            var result = _kscHrContext.JobPositionNatureSubGroups.Include(x => x.Chart_JobPositionNature).AsQueryable();
            return result;
        }

    }
}

using Ksc.Hr.Domain.Entities;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Repositories.EmployeeBase;
using KSC.Infrastructure.Persistance;

namespace Ksc.HR.Data.Persistant.Repositories.EmployeeBase
{
    public class RegionRepository : EfRepository<Region, int>, IRegionRepository
    {
        private readonly KscHrContext _kscHrContext;
        public RegionRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<Region> GetRegionById(int id)
        {
            return _kscHrContext.Region.Where(a => a.Id == id);
        }
        public IQueryable<Region> GetRegions()
        {
            var result = _kscHrContext.Region.AsQueryable();
            return result;
        }
    }
}

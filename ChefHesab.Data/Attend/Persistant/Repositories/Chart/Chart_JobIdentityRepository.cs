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
    public class Chart_JobIdentityRepository : EfRepository<Chart_JobIdentity, int>, IChart_JobIdentityRepository
    {
        private readonly KscHrContext _kscHrContext;
        public Chart_JobIdentityRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<Chart_JobIdentity> GetChart_JobIdentityByCode(string code)
        {
            return _kscHrContext.Chart_JobIdentity.Where(a => a.Code == code).AsNoTracking();
        }

        public IQueryable<Chart_JobIdentity> GetChart_JobIdentityById(int id)
        {
            return  _kscHrContext.Chart_JobIdentity.Where(a => a.Id == id).AsNoTracking();
        }
        public IQueryable<Chart_JobIdentity> GetChart_JobIdentites()
        {
            var result = _kscHrContext.Chart_JobIdentity.AsQueryable().AsNoTracking();
            return result;
        }

        public IQueryable<Chart_JobIdentity> GetAllIncludeJobCategory()
        {
            var result = _kscHrContext.Chart_JobIdentity.Include(x=>x.Chart_JobCategory).AsQueryable().AsNoTracking();
            return result;
        }
    }
}

using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Chart;
using Ksc.HR.Domain.Repositories.Chart;
using KSC.Infrastructure.Persistance;

namespace Ksc.HR.Data.Persistant.Repositories.Chart
{
    public class Chart_JobCategoryDefinationRepository : EfRepository<Chart_JobCategoryDefination, int>, IChart_JobCategoryDefinationRepository
    {
        private readonly KscHrContext _kscHrContext;
        public Chart_JobCategoryDefinationRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }


        public IQueryable<Chart_JobCategoryDefination> GetChart_JobCategoryDefinationById(int id)
        {
            return _kscHrContext.Chart_JobCategoryDefination.Where(a => a.Id == id);
        }

        public IQueryable<Chart_JobCategoryDefination> GetChart_JobCategoryDefinations()
        {
            var result = _kscHrContext.Chart_JobCategoryDefination.AsQueryable();
            return result;
        }

    }
}

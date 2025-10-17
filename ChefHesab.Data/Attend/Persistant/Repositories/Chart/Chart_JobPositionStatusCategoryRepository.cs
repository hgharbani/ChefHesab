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
    public class Chart_JobPositionStatusCategoryRepository : EfRepository<Chart_JobPositionStatusCategory, int>, IChart_JobPositionStatusCategoryRepository
    {
        private readonly KscHrContext _kscHrContext;
        public Chart_JobPositionStatusCategoryRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }


        public IQueryable<Chart_JobPositionStatusCategory> GetChart_JobPositionStatusCategoryById(int id)
        {
            return  _kscHrContext.Chart_JobPositionStatusCategory.Where(a => a.Id == id).AsNoTracking();
        }
        public IQueryable<Chart_JobPositionStatusCategory> GetChart_JobPositionStatusCategorys()
        {
            var result = _kscHrContext.Chart_JobPositionStatusCategory.AsQueryable().AsNoTracking();
            return result;
        }
    }
}

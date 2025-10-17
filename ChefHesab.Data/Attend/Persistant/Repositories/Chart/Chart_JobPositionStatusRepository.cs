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
    public class Chart_JobPositionStatusRepository : EfRepository<Chart_JobPositionStatus, int>, IChart_JobPositionStatusRepository
    {
        private readonly KscHrContext _kscHrContext;
        public Chart_JobPositionStatusRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<Chart_JobPositionStatus> GetAllIncludeCategory()
        {
            var result = _kscHrContext.Chart_JobPositionStatus.Include(x=>x.Chart_JobPositionStatusCategory).AsQueryable().AsNoTracking();
            return result;
        }

        public IQueryable<Chart_JobPositionStatus> GetChart_JobPositionStatusById(int id)
        {
            return  _kscHrContext.Chart_JobPositionStatus.Where(a => a.Id == id).AsNoTracking();
        }
        public IQueryable<Chart_JobPositionStatus> GetChart_JobPositionStatuses()
        {
            var result = _kscHrContext.Chart_JobPositionStatus.AsQueryable().AsNoTracking();
            return result;
        }
    }
}

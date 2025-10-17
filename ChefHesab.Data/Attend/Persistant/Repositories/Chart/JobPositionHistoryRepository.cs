using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Chart;
using Ksc.HR.Domain.Repositories.Chart;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Ksc.HR.Data.Persistant.Repositories.Chart
{
    public class JobPositionHistoryRepository: EfRepository<Chart_JobPositionHistory,long>,IJobPositionHistoryRepository
    {
        private readonly KscHrContext _kscHrContext;
        public JobPositionHistoryRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }


        public IQueryable<Chart_JobPositionHistory> GetJobPositionHistoryById(long id)
        {
            return _kscHrContext.Chart_JobPositionHistory.Where(a => a.Id == id)
                .Include(x=>x.JobIdentity)
                .Include(x=>x.JobPositionNature)
                .Include(x=>x.Chart_Structure)
                .Include(x=>x.JobPositionStatus)
                .Include(x=>x.RewardSpecific)
                .Include(x=>x.JobPosition)
                .AsNoTracking();
        }

        public IQueryable<Chart_JobPositionHistory> GetJobPositionHistorys()
        {
            var result = _kscHrContext.Chart_JobPositionHistory.AsQueryable().AsNoTracking();
            return result;
        }
        public IQueryable<Chart_JobPositionHistory> GetJobPositionHistoryByJobIds(int id)
        {
            var result = _kscHrContext.Chart_JobPositionHistory.Where(x=>x.JobPositionId==id).AsQueryable().AsNoTracking();
            return result;
        }
    }
}

using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities;
using Ksc.Hr.Domain.Repositories;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Reward;
using Ksc.HR.Domain.Repositories.Reward;
using Microsoft.EntityFrameworkCore;
namespace Ksc.Hr.Data.Persistant.Repositories
{
    public partial class RewardInQualityControlMonthlyProductionRepository : EfRepository<RewardInQualityControlMonthlyProduction, int>, IRewardInQualityControlMonthlyProductionRepository
    {
        private readonly KscHrContext _kscHrContext;
        public RewardInQualityControlMonthlyProductionRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;

        
        }

        public IQueryable<RewardInQualityControlMonthlyProduction> GetRewardInQualityControlMonthlyProductionByYearMonth(int rewardInId)
        {
            var query = _kscHrContext.RewardInQualityControlMonthlyProductions
                .Where(x => x.RewardInId == rewardInId)
                .AsNoTracking();
            return query;
        }
        public IQueryable<RewardInQualityControlMonthlyProduction> GetRewardInQualityControlMonthlyProduction(int yearMonth)
        {
            var query = _kscHrContext.RewardInQualityControlMonthlyProductions.Include(x=>x.RewardIn)
                .Where(x => x.RewardIn.YearMonth == yearMonth)
                .AsNoTracking();
            return query;
        }
    }
}


using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities;
using Ksc.Hr.Domain.Repositories;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Reward;
using Ksc.HR.Domain.Repositories.Reward;
using Microsoft.EntityFrameworkCore;
namespace Ksc.Hr.Data.Persistant.Repositories
{
    public partial class RewardOutHeaderRepository : EfRepository<RewardOutHeader, int>, IRewardOutHeaderRepository
    {
        private readonly KscHrContext _kscHrContext;
        public RewardOutHeaderRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        
        public IQueryable<RewardOutHeader> GetOutHeaderByYearMonth(int yearmonth, int rewardCategoryId)
        {
            var query = _kscHrContext.RewardOutHeaders
                .Where(x => x.YearMonth == yearmonth && x.RewardCategoryId == rewardCategoryId)
                .AsNoTracking();
            return query;
        }
        public RewardOutHeader GetOneOutHeaderByYearMonth(int yearmonth, int rewardCategoryId)
        {
            var query = _kscHrContext.RewardOutHeaders
                .FirstOrDefault(x => x.YearMonth == yearmonth && x.RewardCategoryId == rewardCategoryId);      
            return query;
        }
        public IQueryable<RewardOutHeader> GetOutHeaderByRewardCategoryId(int yearmonth,int rewardCategoryId)
        {
            var query = _kscHrContext.RewardOutHeaders.Include(x=>x.RewardOutDetails)
                .Where(x => x.YearMonth == yearmonth && x.RewardCategoryId==rewardCategoryId)
                .AsNoTracking();
            return query;
        }

        public IQueryable<RewardOutHeader> GetOutHeaderByRelated(int yearmonth, int rewardCategoryId)
        {
            var query = _kscHrContext.RewardOutHeaders
                .Include(x=>x.RewardOutDetails)
                .ThenInclude(x=>x.ProductionEfficiency)
                .Include(x => x.RewardOutDetails)
                .ThenInclude(x => x.RewardUnitType)

                .Where(x => x.YearMonth == yearmonth && x.RewardCategoryId== rewardCategoryId)
                .AsNoTracking();
            return query;
        }
    }
}


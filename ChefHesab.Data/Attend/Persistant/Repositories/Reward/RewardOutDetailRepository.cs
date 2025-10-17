using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities;
using Ksc.Hr.Domain.Repositories;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Reward;
using Ksc.HR.Domain.Repositories.Reward;
using Microsoft.EntityFrameworkCore;
namespace Ksc.Hr.Data.Persistant.Repositories
{
    public partial class RewardOutDetailRepository : EfRepository<RewardOutDetail, int>, IRewardOutDetailRepository
    {
        private readonly KscHrContext _kscHrContext;
        public RewardOutDetailRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<RewardOutDetail> GetProductionRewardReport(int yearMonth,int rewardCategoryId)
        {
            var resault = _kscHrContext
                .RewardOutDetails
                .Include(x=>x.RewardOutHeader)
                .Include(x=>x.ProductionEfficiency)
                .Where(x =>
                x.RewardOutHeader.YearMonth == yearMonth &&
                x.RewardOutHeader.RewardCategoryId == rewardCategoryId );

            return resault;
        }


        public IQueryable<RewardOutDetail> GetSpecialRewardReport(int yearMonth, int rewardCategoryId)
        {
            var resault = _kscHrContext
                .RewardOutDetails
                .Include(x => x.RewardOutHeader)
                .Include(x => x.RewardUnitType)
                .Where(x =>
                x.RewardOutHeader.YearMonth == yearMonth &&
                x.RewardOutHeader.RewardCategoryId == rewardCategoryId);

            return resault;
        }
    }
}


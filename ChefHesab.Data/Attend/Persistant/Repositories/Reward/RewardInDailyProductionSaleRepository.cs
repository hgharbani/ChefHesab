using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities;
using Ksc.Hr.Domain.Repositories;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Reward;
using Ksc.HR.Domain.Repositories.Reward;
using Microsoft.EntityFrameworkCore;
namespace Ksc.Hr.Data.Persistant.Repositories
{
    public partial class RewardInDailyProductionSaleRepository : EfRepository<RewardInDailyProductionSale, int>, IRewardInDailyProductionSaleRepository
    {
        private readonly KscHrContext _kscHrContext;
        public RewardInDailyProductionSaleRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        //دیتای خام تولید روزانه به ازای rewardin که سال/ماه دارد
        public IQueryable<RewardInDailyProductionSale> GetRewardInDailyProductionSale(int yearmonth)
        {
            var query = _kscHrContext.RewardInDailyProductionSales.Include(x => x.RewardIn)
                .Where(x => x.RewardIn.YearMonth == yearmonth);

            return query;
        }

        public IQueryable<RewardInDailyProductionSale> GetRewardInDailyProductionSaleByUnitTypeId
            (int yearmonth,int unitTypeId,int daysale)
        {
            var query = _kscHrContext.RewardInDailyProductionSales.Include(x => x.RewardIn)
                .Where(x => x.RewardIn.YearMonth == yearmonth && x.RewardUnitTypeId== unitTypeId && x.DaySale==daysale);

            return query;
        }
    }
}


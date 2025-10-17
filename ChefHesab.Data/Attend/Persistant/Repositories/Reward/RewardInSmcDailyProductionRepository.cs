using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities;
using Ksc.Hr.Domain.Repositories;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Reward;
using Ksc.HR.Domain.Repositories.Reward;
using Ksc.HR.Domain.Entities.Pay;
using Microsoft.EntityFrameworkCore;
using Ksc.HR.Share.Model.Reward;
using Ksc.HR.DTO.Reward.RewardInSmcDailyProduction;
using Ksc.HR.Share.Model.Rule;
namespace Ksc.Hr.Data.Persistant.Repositories
{
    public partial class RewardInSmcDailyProductionRepository : EfRepository<RewardInSmcDailyProduction, int>, IRewardInSmcDailyProductionRepository
    {
        private readonly KscHrContext _kscHrContext;
        public RewardInSmcDailyProductionRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        //دیتای خام تولید روزانه به ازای rewardin که سال/ماه دارد
        public IQueryable<RewardInSmcDailyProduction> GetRewardInSmcDailyProduction(int yearmonth)
        {
            var query = _kscHrContext.RewardInSmcDailyProductions.Include(x=>x.RewardIn)
                .Where(x => x.RewardIn.YearMonth == yearmonth)
                ;

             return query;
        }

        public long SumProductionSmcWeightByYearMonth(int yearMonth)
        {

            long result = 0;
            try
            {
                var RewardIn = _kscHrContext.RewardIns.Where(x => x.YearMonth == yearMonth).FirstOrDefault();
                if (RewardIn != null)
                {
                    var query = _kscHrContext.RewardInSmcDailyProductions.Include(x => x.RewardIn)
                    .Where(x => x.RewardIn.YearMonth == yearMonth
                    )
                        .Select(x => new
                        {
                            ProductionSmcWeightItem = x.ProductionSmcWeight,
                        });
                    var SumProductionSmcWeightByYearMonth = query.Select(x => x.ProductionSmcWeightItem).Sum() ?? 0;

                    result = SumProductionSmcWeightByYearMonth;
                }
            }
            catch (Exception ex)
            {
                return result;

            }

            return result;

        }


        public long SumPlanSmcWeightByYearMonth(int yearMonth)
        {

            long result = 0;
            try
            {
                var RewardIn = _kscHrContext.RewardIns.Where(x => x.YearMonth == yearMonth).FirstOrDefault();
                if (RewardIn != null)
                {
                    var query = _kscHrContext.RewardInSmcDailyProductions.Include(x => x.RewardIn)
                    .Where(x => x.RewardIn.YearMonth == yearMonth
                    )
                        .Select(x => new
                        {
                            PlanSmcWeightItem = x.PlanSmcWeight,
                        });
                    var SumPlanSmcWeight = query.Select(x => x.PlanSmcWeightItem).Sum() ?? 0;

                    result = SumPlanSmcWeight;
                }
            }
            catch (Exception ex)
            {
                return result;

            }

            return result;

        }

        
        
   
    }
}


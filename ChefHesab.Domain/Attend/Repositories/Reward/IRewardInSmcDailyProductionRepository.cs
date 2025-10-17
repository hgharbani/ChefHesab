using KSC.Domain;
using Ksc.HR.Domain.Entities.Salary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Domain.Entities.Rule;
using Ksc.HR.Domain.Entities.Reward;

namespace Ksc.HR.Domain.Repositories.Reward
{
    public interface IRewardInSmcDailyProductionRepository : IRepository<RewardInSmcDailyProduction, int>
    {
        IQueryable<RewardInSmcDailyProduction> GetRewardInSmcDailyProduction(int rewardInId);
        long SumPlanSmcWeightByYearMonth(int yearMonth);
        long SumProductionSmcWeightByYearMonth(int yearMonth);
    }
}

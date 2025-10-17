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
    public interface IRewardInQualityControlMonthlyProductionRepository : IRepository<RewardInQualityControlMonthlyProduction, int>
    {
        IQueryable<RewardInQualityControlMonthlyProduction> GetRewardInQualityControlMonthlyProduction(int yearMonth);
        IQueryable<RewardInQualityControlMonthlyProduction> GetRewardInQualityControlMonthlyProductionByYearMonth(int rewardInId);
    }
}

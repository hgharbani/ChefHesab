using KSC.Domain;
using Ksc.HR.Domain.Entities.Salary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Domain.Entities.Rule;
using Ksc.HR.Domain.Entities.Reward;
using Ksc.HR.Share.Model.Reward;

namespace Ksc.HR.Domain.Repositories.Reward
{
    public interface IRewardBaseSpecificOfUnitHeaderRepository : IRepository<RewardBaseSpecificOfUnitHeader, int>
    {
        RewardBaseSpecificOfUnitHeader GetRewardBaseSpecificOfUnitHeaderByYearMonth(int yearMonth);
        IQueryable<RewardBaseSpecificOfUnitHeader> GetRewardBaseSpecificOfUnitHeaderByRelated(int yearMonth, int rewardUnitTypeId);

        int GetHeaderIdBybasisId(int basisId, int rewardUnitTypeId,bool IsMonthly);
        IQueryable<RewardBaseSpecificOfUnitHeader> GetRewardBaseSpecificDetailByRelated(int yearMonth, int rewardUnitTypeId);
        RewardBaseSpecificOfUnitHeader GetLatestDataForFinal(int yearMonth, int rewardUnitTypeId);
        Tuple<List<RewardBaseSpecificOfUnitHeader>, RewardBaseSpecific, List<MaxYearMonthHeaderByUnitVM>> GetLatestDataForCreate(List<int> rewardUnitTypeId);
        int GetMaxYearMonth(int yearMonth, int rewardUnitTypeId);
    }
}

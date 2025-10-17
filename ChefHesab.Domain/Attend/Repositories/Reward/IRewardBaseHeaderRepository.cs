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
    public interface IRewardBaseHeaderRepository : IRepository<RewardBaseHeader, int>
    {
        RewardBaseHeader GetByValidStartDate(int? validStartDate);
        RewardBaseHeader GetByValidStartDateQuery(int? yearMonth);
        RewardBaseHeader GetLatest();
        RewardBaseHeader GetRewardBaseHeaderByYearMonth(int yearMonth);
        IQueryable<RewardBaseHeader> GetRewardBaseHeaders(RewardBaseHeaderSearchModel searchModel);
    }
}

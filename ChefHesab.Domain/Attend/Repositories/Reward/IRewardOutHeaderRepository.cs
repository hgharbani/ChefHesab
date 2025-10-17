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
    public interface IRewardOutHeaderRepository : IRepository<RewardOutHeader, int>
    {
        RewardOutHeader GetOneOutHeaderByYearMonth(int yearmonth, int rewardCategoryId);
        IQueryable<RewardOutHeader> GetOutHeaderByRelated(int yearmonth, int rewardCategoryIds);
        IQueryable<RewardOutHeader> GetOutHeaderByRewardCategoryId(int yearmonth, int rewardCategoryId);
        IQueryable<RewardOutHeader> GetOutHeaderByYearMonth(int yearmonth, int rewardCategoryId);
    }
}

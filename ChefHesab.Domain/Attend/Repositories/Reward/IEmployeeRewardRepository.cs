using KSC.Domain;
using Ksc.HR.Domain.Entities.Reward;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Share.Model.EmployeeReward;

namespace Ksc.HR.Domain.Repositories.Reward
{
    public interface IEmployeeRewardRepository : IRepository<EmployeeReward, long>
    {
        IQueryable<EmployeeReward> GetEmployeeRewards(int yearmonth, int rewardCategoryId);
        List<EmployeeRewardDto> GetEmployeesWithCalculateRewards(EmployeeRewardInputModel model);
    }
}

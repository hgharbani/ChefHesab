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
    public interface IRewardBaseSpecificRepository : IRepository<RewardBaseSpecific, int>
    {
        int GetIdByyear(int year);


        /// <summary>
        /// فیلتر دیتا بر اساس سال ماه واحد- امکان افزودن جداول وابسته بر اساس ورودی متد
        /// </summary>
        /// <param name="yearMonth">getlatest = 0 or 140301</param>
        /// <param name="include">Header / HeaderDetail</param>
        /// <param name="asNoTracking"></param>
        /// <returns></returns>
        IQueryable<RewardBaseSpecific> GetRewardBaseSpecifics(int? yearMonth, int? unitTypeIdInHeader, bool asNoTracking = false, string include = null);
        public IQueryable<RewardBaseSpecific> GetRewardBaseSpecificByYearMonth(int yearMonth);

        IQueryable<RewardBaseSpecific> GetRewardBaseSpecificByRelated(int yearMonth, int rewardUnitTypeId);
    }
}

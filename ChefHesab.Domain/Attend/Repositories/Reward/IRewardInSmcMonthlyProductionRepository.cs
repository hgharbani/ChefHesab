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
    public interface IRewardInSmcMonthlyProductionRepository : IRepository<RewardInSmcMonthlyProduction, int>
    {
        List<EmployeeRewardSpecificDto> ActiveEmployeeCount();
        IQueryable<RewardInSmcMonthlyProduction> GetRewardInSmcMonthlyProduction(int yearmonth);
        double? DivPaymentAmountKpi(int yearMonth, int rewardUnitTypeId);
        long SumActualWeightZamzamByYearMonth(int yearMonth);
        long SumActualWeightZamzamVaEhyaByYearMonth(int yearMonth);
        List<EmployeeRewardSpecificDto> SumTotalPercentPerJob();
        Tuple<long, long> SumActualWeightAndPlanZamzam(int yearMonth);
    }
}

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
    public interface IRewardInMonthlyProductionSaleRepository : IRepository<RewardInMonthlyProductionSale, int>
    {
        ClacToolidMoadelViewModel GetClacToolidMoadelViewModel(int yearMonth);
        IQueryable<RewardInDailyProductionSale> GetRewardInDailyProductionSale(int yearmonth, int rewardUnitTypeId);
        //double GetToolidMoadel(int yearMonth);
    }
}

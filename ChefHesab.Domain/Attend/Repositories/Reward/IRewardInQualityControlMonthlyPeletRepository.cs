using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Domain.Entities.Reward;
using Ksc.HR.Share.Model.Reward;

namespace Ksc.HR.Domain.Repositories.Reward
{
    public interface IRewardInQualityControlMonthlyPeletRepository : IRepository<RewardInQualityControlMonthlyPelet, int>
    {
        IQueryable<RewardInQualityControlMonthlyPelet> GetByRewardInId(int RewardInId);
        IQueryable<RewardInQualityControlMonthlyPelet> GetRewardInQualityControlMonthlyPeletById(int productionEfficiencyId);
        QualityFactorViewModel GetQualityFactorByYearMonth(int yearMonth);
        IQueryable<RewardInQualityControlMonthlyPelet> GetRewardInQualityControlMonthlyPeletByYearMonth(int yearMonth);
    }
}

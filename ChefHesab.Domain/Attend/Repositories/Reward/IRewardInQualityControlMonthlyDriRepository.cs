using KSC.Domain;
using Ksc.HR.Domain.Entities.Reward;
namespace Ksc.HR.Domain.Repositories.Reward
{
    public interface IRewardInQualityControlMonthlyDriRepository : IRepository<RewardInQualityControlMonthlyDri, int>
    {
        IQueryable<RewardInQualityControlMonthlyDri> GetRewardInQualityControlMonthlyDriById(int productionEfficiencyId);
        IQueryable<RewardInQualityControlMonthlyDri> GetRewardInQualityControlMonthlyDriByYearMonth(int yearMonth);
        double SumQualityControlMonthlyDriByYearMonth(int yearMonth);
        double SumQualityControlMonthlyDriByYearMonthForSpecific(int yearMonth);
        double SumQualityControlMonthlyDriSpecificByYearMonth(int yearMonth);
    }
}

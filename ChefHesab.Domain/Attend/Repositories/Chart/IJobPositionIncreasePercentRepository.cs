using Ksc.HR.Domain.Entities.Chart;
using KSC.Domain;

namespace Ksc.HR.Domain.Repositories.Chart
{
    public interface IJobPositionIncreasePercentRepository : IRepository<Chart_JobPositionIncreasePercent, int>
    {
        Chart_JobPositionIncreasePercent FindRepeateJobpositionScore(int jobpositionId, double CoefficientYearsDay, double CoefficientYearsShiftc, DateTime startDate, DateTime? endDate);
        IQueryable<Chart_JobPositionIncreasePercent> GetJobPositionIncreasePercentByCode(string code);
        IQueryable<Chart_JobPositionIncreasePercent> GetJobPositionIncreasePercentById(int id);
        IQueryable<Chart_JobPositionIncreasePercent> GetJobPositionIncreasePercents();
        IQueryable<Chart_JobPositionIncreasePercent> GetJobPositionIncrese(int jobPositionId);
        bool GetRepeateJobpositionScore(int jobpositionId, double CoefficientYearsDay, double CoefficientYearsShiftc, DateTime startDate, DateTime? endDate);
    }
}

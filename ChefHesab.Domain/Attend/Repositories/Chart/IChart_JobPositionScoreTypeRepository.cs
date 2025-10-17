using Ksc.HR.Domain.Entities.Chart;
using KSC.Domain;

namespace Ksc.HR.Domain.Repositories.Chart
{

    public interface IChart_JobPositionScoreTypeRepository : IRepository<Chart_JobPositionScoreType, int>
    {
        //IQueryable<Chart_JobPositionScoreType> GetChart_JobPositionScoreTypeById(int id);
        // IQueryable<Chart_JobPositionScoreType> GetChart_JobCategoryDefinations();
        IQueryable<Chart_JobPositionScoreType> GetAllData();
        int GetScoreTypeIdByCode(int? code);

    }
}
using Ksc.HR.Domain.Entities.Chart;
using KSC.Domain;

namespace Ksc.HR.Domain.Repositories.Chart
{
    public interface IChart_JobCategoryDefinationRepository : IRepository<Chart_JobCategoryDefination, int>
    {
        IQueryable<Chart_JobCategoryDefination> GetChart_JobCategoryDefinationById(int id);
        IQueryable<Chart_JobCategoryDefination> GetChart_JobCategoryDefinations();
    }
}
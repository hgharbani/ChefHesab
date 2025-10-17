using Ksc.HR.Domain.Entities.Chart;
using KSC.Domain;

namespace Ksc.HR.Domain.Repositories.Chart
{

    public interface IChart_JobPositionFieldRepository : IRepository<Chart_JobPositionField, int>
    {

        //IQueryable<IChart_JobPositionField> GetAllData();
        IQueryable<Chart_JobPositionField> GetAllData();
    }
}
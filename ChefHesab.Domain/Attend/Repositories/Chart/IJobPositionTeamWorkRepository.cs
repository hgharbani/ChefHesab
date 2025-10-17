using Ksc.HR.Domain.Entities.Chart;
using KSC.Domain;

namespace Ksc.HR.Domain.Repositories.Chart;

public interface IJobPositionTeamWorkRepository : IRepository<Chart_JobPositionTeamWork>
{
    Chart_JobPositionTeamWork GetOneByRelations(int id);

    Chart_JobPositionTeamWork GetOneByRelationsBasedOnJobPositionId(int id);
}

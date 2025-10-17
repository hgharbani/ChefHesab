using Ksc.HR.Domain.Entities.StandBy;
using KSC.Domain;

namespace Ksc.HR.Domain.Repositories.StandBy;

public interface IStandbyReplacementRequestsRepository : IRepository<StandbyReplacementRequests, int>
{
    StandbyReplacementRequests GetByRelations(int wfRequestId);
}

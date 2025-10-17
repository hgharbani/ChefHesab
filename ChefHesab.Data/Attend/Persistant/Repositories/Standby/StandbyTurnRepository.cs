using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.StandBy;
using Ksc.HR.Domain.Repositories.StandBy;
using KSC.Infrastructure.Persistance;

namespace Ksc.HR.Data.Persistant.Repositories.Standby;

public class StandbyTurnRepository : EfRepository<StandbyTurn, byte>, IStandbyTurnRepository
{
    private readonly KscHrContext _context;

    public StandbyTurnRepository(KscHrContext context) : base(context)
    {
        _context = context;
    }
}

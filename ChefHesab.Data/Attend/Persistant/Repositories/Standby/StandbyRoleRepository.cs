using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.StandBy;
using Ksc.HR.Domain.Repositories.StandBy;
using KSC.Infrastructure.Persistance;

namespace Ksc.HR.Data.Persistant.Repositories.Standby;

public class StandbyRoleRepository : EfRepository<StandbyRole, byte>, IStandbyRoleRepository
{
    private readonly KscHrContext _context;

    public StandbyRoleRepository(KscHrContext context) : base(context)
    {
        _context = context;
    }
}

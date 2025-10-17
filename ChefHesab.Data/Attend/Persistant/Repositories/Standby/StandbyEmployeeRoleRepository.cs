using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.StandBy;
using Ksc.HR.Domain.Repositories.StandBy;
using KSC.Infrastructure.Persistance;

namespace Ksc.HR.Data.Persistant.Repositories.Standby;

public class StandbyEmployeeRoleRepository : EfRepository<StandbyEmployeeRole, int>, IStandbyEmployeeRoleRepository
{
    private readonly KscHrContext _context;

    public StandbyEmployeeRoleRepository(KscHrContext context) : base(context)
    {
        _context = context;
    }
}

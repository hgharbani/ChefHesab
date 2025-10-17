using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.StandBy;
using Ksc.HR.Domain.Repositories.StandBy;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Ksc.HR.Data.Persistant.Repositories.Standby;

public class StandbyReplacementRequestsRepository : EfRepository<StandbyReplacementRequests, int>, IStandbyReplacementRequestsRepository
{
    private readonly KscHrContext _context;
    public StandbyReplacementRequestsRepository(KscHrContext context) : base(context)
    {
        _context = context;
    }

    public StandbyReplacementRequests GetByRelations(int wfRequestId)
    {
        return _context
            .StandbyReplacementRequests
            .Include(x => x.SourceStandbyEmployeeBoard)
            .ThenInclude(x => x.WorkCalendar)
            .Include(x => x.SourceStandbyEmployeeBoard)
            .ThenInclude(x => x.StandbyEmployeeRole)
            .ThenInclude(x => x.Employee)
            .Include(x => x.DestinationStandbyEmployeeBoard)
            .ThenInclude(x => x.WorkCalendar)
            .Include(x => x.DestinationStandbyEmployeeBoard)
            .ThenInclude(x => x.StandbyEmployeeRole)
            .ThenInclude(x => x.Employee)
            .Include(x => x.SourceStandbyEmployeeBoard)
            .ThenInclude(x => x.StandbyTurn)
            .Include(x => x.DestinationStandbyEmployeeBoard)
            .ThenInclude(x => x.StandbyTurn)
            .Include(x => x.SourceStandbyEmployeeBoard)
            .ThenInclude(x => x.StandbyEmployeeRole)
            .ThenInclude(x => x.StandbyRole)
            .Include(x => x.DestinationStandbyEmployeeBoard)
            .ThenInclude(x => x.StandbyEmployeeRole)
            .ThenInclude(x => x.StandbyRole)
            .FirstOrDefault(x => x.WfRequestId == wfRequestId);
    }
}

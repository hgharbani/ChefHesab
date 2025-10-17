using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Chart;
using Ksc.HR.Domain.Repositories.Chart;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Ksc.HR.Data.Persistant.Repositories.Chart;

public class JobPositionTeamWorkRepository : EfRepository<Chart_JobPositionTeamWork, int>, IJobPositionTeamWorkRepository
{
    private readonly KscHrContext _context;

    public JobPositionTeamWorkRepository(KscHrContext context) : base(context)
    {
        _context = context;
    }

    public Chart_JobPositionTeamWork GetOneByRelations(int id)
    {
        var query = _context
            .JobPositionTeamWorks
            .Include(x => x.TeamWork)
            .Include(x => x.Chart_JobPosition)
            .FirstOrDefault(x => x.Id == id);

        return query;
    }

    public Chart_JobPositionTeamWork GetOneByRelationsBasedOnJobPositionId(int id)
    {
        var query = _context
            .JobPositionTeamWorks
            .Include(x => x.TeamWork)
            .Include(x => x.Chart_JobPosition)
            .FirstOrDefault(x => x.JobPositionId == id);

        return query;
    }
}

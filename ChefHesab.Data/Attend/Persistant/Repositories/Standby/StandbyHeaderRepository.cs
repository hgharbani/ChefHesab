using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.StandBy;
using Ksc.HR.Domain.Repositories.StandBy;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Ksc.HR.Data.Persistant.Repositories.Standby;

public class StandbyHeaderRepository : EfRepository<StandbyHeader, int>, IStandbyHeaderRepository
{
    private readonly KscHrContext _kscHrContext;

    public StandbyHeaderRepository(KscHrContext KscHrContext) : base(KscHrContext)
    {
        _kscHrContext = KscHrContext;
    }

    public StandbyHeader GetByYearMonthIncluded(int yearMonth)
    {
        return _kscHrContext.StandbyHeader.Where(x => x.YearMonth == yearMonth).Include(x => x.WF_Request).FirstOrDefault();
    }
}

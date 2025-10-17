using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.View;
using Ksc.HR.Domain.Repositories.View;
using KSC.Infrastructure.Persistance;

namespace Ksc.HR.Data.Persistant.Repositories.View;

public sealed class ViewMisPayrollRepository : EfRepository<ViewMisPayroll>, IViewMisPayrollRepository
{
    private readonly KscHrContext _context;

    public ViewMisPayrollRepository(KscHrContext context) : base(context)
    {
        _context = context;
    }
}

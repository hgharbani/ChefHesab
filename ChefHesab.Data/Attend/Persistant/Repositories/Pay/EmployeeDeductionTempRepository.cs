using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Pay;
using Ksc.HR.Domain.Repositories.Pay;
using KSC.Infrastructure.Persistance;

namespace Ksc.HR.Data.Persistant.Repositories.Pay;

public sealed class EmployeeDeductionTempRepository : EfRepository<EmployeeDeductionTemp, long>, IEmployeeDeductionTempRepository
{
    private readonly KscHrContext _context;

    public EmployeeDeductionTempRepository(KscHrContext context) : base(context)
    {
        _context = context;
    }
}

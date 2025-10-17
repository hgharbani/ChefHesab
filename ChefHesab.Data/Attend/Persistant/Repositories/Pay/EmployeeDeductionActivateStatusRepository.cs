using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Pay;
using Ksc.HR.Domain.Repositories.Pay;
using KSC.Infrastructure.Persistance;

namespace Ksc.HR.Data.Persistant.Repositories.Pay;

public class EmployeeDeductionActivateStatusRepository : EfRepository<EmployeeDeductionActivateStatus, int>, IEmployeeDeductionActivateStatusRepository
{
    public EmployeeDeductionActivateStatusRepository(KscHrContext context) : base(context)
    {

    }
}

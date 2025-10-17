using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Emp;
using Ksc.HR.Domain.Repositories.Emp;
using KSC.Infrastructure.Persistance;


namespace Ksc.HR.Data.Persistant.Repositories.Emp
{
    public class EmployeeHistoryTypeEmployementRepository : EfRepository<EmployeeHistoryTypeEmployement, int>, IEmployeeHistoryTypeEmployementRepository
    {
        private readonly KscHrContext _kscHrContext;
        public EmployeeHistoryTypeEmployementRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
    }
}

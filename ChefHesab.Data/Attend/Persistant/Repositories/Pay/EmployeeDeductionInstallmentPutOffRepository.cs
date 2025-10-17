using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities;
using Ksc.Hr.Domain.Repositories;
using Ksc.HR.Domain.Entities.Pay;
using Ksc.HR.Data.Persistant.Context;
namespace Ksc.Hr.Data.Persistant.Repositories
{
    public partial class EmployeeDeductionInstallmentPutOffRepository : EfRepository<EmployeeDeductionInstallmentPutOff, long>, IEmployeeDeductionInstallmentPutOffRepository
    {
        private readonly KscHrContext _kscHrContext;
        public EmployeeDeductionInstallmentPutOffRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
    }
}


using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.EmployeeBase;
using Ksc.HR.Domain.Repositories.EmployeeBase;
using KSC.Infrastructure.Persistance;

namespace Ksc.HR.Data.Persistant.Repositories.EmployeeBase
{
    public class InsuranceTypeRepository : EfRepository<InsuranceType, int>, IInsuranceTypeRepository
    {
        private readonly KscHrContext _kscHrContext;
        public InsuranceTypeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<InsuranceType> GetAllInsuranceTypeNoTracking(int id)
        {
            return _kscHrContext.InsuranceTypes.Where(a => a.Id == id);
        }
    }
}

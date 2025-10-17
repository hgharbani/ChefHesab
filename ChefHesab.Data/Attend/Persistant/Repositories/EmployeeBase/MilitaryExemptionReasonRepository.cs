using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.EmployeeBase;
using Ksc.HR.Domain.Repositories.EmployeeBase;
using KSC.Infrastructure.Persistance;

namespace Ksc.HR.Data.Persistant.Repositories.EmployeeBase
{
    public class MilitaryExemptionReasonRepository : EfRepository<MilitaryExemptionReason, int>, IMilitaryExemptionReasonRepository
    {
        private readonly KscHrContext _kscHrContext;
        public MilitaryExemptionReasonRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<MilitaryExemptionReason> GetAllMilitaryExemptionReasonNoTracking(int id)
        {
            return _kscHrContext.MilitaryExemptionReasons.Where(a => a.Id == id);
        }
    }
}

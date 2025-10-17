using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.EmployeeBase;
using Ksc.HR.Domain.Repositories.EmployeeBase;
using KSC.Infrastructure.Persistance;

namespace Ksc.HR.Data.Persistant.Repositories.EmployeeBase
{
    public class MilitaryUnitRepository : EfRepository<MilitaryUnit, int>, IMilitaryUnitRepository
    {
        private readonly KscHrContext _kscHrContext;
        public MilitaryUnitRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<MilitaryUnit> GetAllMilitaryUnitNoTracking(int id)
        {
            return _kscHrContext.MilitaryUnits.Where(a => a.Id == id);
        }
    }
}

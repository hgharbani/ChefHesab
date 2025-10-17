using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.EmployeeBase;
using Ksc.HR.Domain.Repositories.EmployeeBase;
using KSC.Infrastructure.Persistance;

namespace Ksc.HR.Data.Persistant.Repositories.EmployeeBase
{
    public class MilitaryDegreeRepository : EfRepository<MilitaryDegree, int>, IMilitaryDegreeRepository
    {
        private readonly KscHrContext _kscHrContext;
        public MilitaryDegreeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<MilitaryDegree> GetAllMilitaryDegreeNoTracking(int id)
        {
            return _kscHrContext.MilitaryDegrees.Where(a => a.Id == id);
        }
    }
}

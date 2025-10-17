using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.EmployeeBase;
using Ksc.HR.Domain.Repositories.EmployeeBase;
using KSC.Infrastructure.Persistance;

namespace Ksc.HR.Data.Persistant.Repositories.EmployeeBase
{
    public class MilitaryTypeRepository : EfRepository<MilitaryType, int>, IMilitaryTypeRepository
    {
        private readonly KscHrContext _kscHrContext;
        public MilitaryTypeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<MilitaryType> GetAllMilitaryTypeNoTracking(int id)
        {
            return _kscHrContext.MilitaryTypes.Where(a => a.Id == id);
        }
    }
}

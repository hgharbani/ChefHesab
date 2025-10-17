using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.EmployeeBase;
using Ksc.HR.Domain.Repositories.EmployeeBase;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Ksc.HR.Data.Persistant.Repositories.EmployeeBase
{
    public class MilitaryStatusRepository : EfRepository<MilitaryStatus, int>, IMilitaryStatusRepository
    {
        private readonly KscHrContext _kscHrContext;
        public MilitaryStatusRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<MilitaryStatus> GetAllMilitaryStatusNoTracking(int id)
        {
            return _kscHrContext.MilitaryStatues.Where(a => a.Id == id);
        }

        public IQueryable<MilitaryStatus> GetDataFromMilitaryStatusForKSCContract()
        {
            return _kscHrContext.MilitaryStatues.IgnoreAutoIncludes();
        }
    }
}

using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Repositories.Personal;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Ksc.HR.Data.Persistant.Repositories.Personal
{
    public class MaritalStatusRepository : EfRepository<MaritalStatus, int>, IMaritalStatusRepository
    {
        private readonly KscHrContext _kscHrContext;
        public MaritalStatusRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<MaritalStatus> GetMaritalStatusById(int id)
        {
            return _kscHrContext.MaritalStatus.Where(a => a.Id == id);
        }
        public IQueryable<MaritalStatus> GetMaritalStatus()
        {
            var result = _kscHrContext.MaritalStatus.AsQueryable();
            return result;
        }

        public IQueryable<MaritalStatus> GetDataFromMaritalStatusForKSCContract()
        {
            return _kscHrContext.MaritalStatus
                .IgnoreAutoIncludes();
        }
    }
}

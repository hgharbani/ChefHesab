using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories.WorkShift;

namespace Ksc.HR.Data.Persistant.Repositories.WorkShift
{
    public class CompanyRepository : EfRepository<Company, int>, ICompanyRepository
    {
        private readonly KscHrContext _kscHrContext;
        public CompanyRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext= KscHrContext;
        }
      
    }
}

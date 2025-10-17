using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities;
using Ksc.Hr.Domain.Repositories;
using Ksc.HR.Domain.Entities.Chart;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Repositories.Chart;

namespace Ksc.Hr.Data.Persistant.Repositories
{
    public partial class LicenseTypeRepository : EfRepository<LicenseType, int>, ILicenseTypeRepository
    {
        private readonly KscHrContext _kscHrContext;
        public LicenseTypeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
    }
}


using KSC.Infrastructure.Persistance;
using Ksc.Hr.Domain.Entities;
using Ksc.Hr.Domain.Repositories;
using Ksc.HR.Data.Persistant.Context;

namespace Ksc.Hr.Data.Persistant.Repositories
{
    public partial class ViewGeneralDataEmployeeWithFamilyForReportRepository : EfRepository<ViewGeneralDataEmployeeWithFamilyForReport>, IViewGeneralDataEmployeeWithFamilyForReportRepository
    {
        private readonly KscHrContext _kscHrContext;
        public ViewGeneralDataEmployeeWithFamilyForReportRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;

        }
    }
}



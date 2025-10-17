  using KSC.Infrastructure.Persistance;
  using Ksc.Hr.Domain.Entities;
  using Ksc.Hr.Domain.Repositories;
using Ksc.HR.Data.Persistant.Context;

namespace Ksc.Hr.Data.Persistant.Repositories
  {
  public partial class ViewEmployeeFamilyReportRepository : EfRepository<ViewEmployeeFamilyReport>,IViewEmployeeFamilyReportRepository
  {
        private readonly KscHrContext _kscHrContext;
        public ViewEmployeeFamilyReportRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;

        }
    }
}


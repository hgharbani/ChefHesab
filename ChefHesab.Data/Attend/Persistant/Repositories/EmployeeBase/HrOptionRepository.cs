  using KSC.Infrastructure.Persistance;
  using Ksc.Hr.Domain.Entities;
  using Ksc.Hr.Domain.Repositories;
using Ksc.HR.Data.Persistant.Context;

namespace Ksc.Hr.Data.Persistant.Repositories
  {
  public partial class HrOptionRepository : EfRepository<HrOption,int>,IHrOptionRepository
  {
        private readonly KscHrContext _kscHrContext;
        public HrOptionRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public HrOption GetptionWithCode(int code)
        {
            return _kscHrContext.HrOptions.FirstOrDefault(o => o.Code == code);
        }
    }
}


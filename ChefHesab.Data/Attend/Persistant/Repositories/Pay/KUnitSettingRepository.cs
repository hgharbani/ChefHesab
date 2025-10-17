using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Pay;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Repositories.Pay;
using KSC.Infrastructure.Persistance;
using System.Linq.Dynamic.Core;

namespace Ksc.HR.Data.Persistant.Repositories.Pay
{

    public class KUnitSettingRepository : EfRepository<KUnitSetting, int>, IKUnitSettingRepository
    {

        private readonly KscHrContext _kscHrContext;
        public KUnitSettingRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public long? GetKUnitByYear(int year)
        {
            return _kscHrContext.KUnitSettings.FirstOrDefault(x => x.Year == year && x.IsActive)?.KUnit;
        }
    }
}

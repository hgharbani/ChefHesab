using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Pay;
using Ksc.HR.Domain.Repositories.Pay;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Ksc.HR.Data.Persistant.Repositories.Pay
{

    public class OtherPaymentSettingParameterValueRepository : EfRepository<OtherPaymentSettingParameterValue, int>, IOtherPaymentSettingParameterValueRepository
    {

        private readonly KscHrContext _kscHrContext;
        public OtherPaymentSettingParameterValueRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<OtherPaymentSettingParameterValue> GetAllQueryableBySettingId(int settingId)
        {
            return _kscHrContext.OtherPaymentSettingParameterValues.AsQueryable().Include(x=>x.OtherPaymentSettingParameter).Where(x => x.OtherPaymentSettingId == settingId);
        }


    }
}

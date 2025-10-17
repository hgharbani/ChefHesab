using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Pay;
using Ksc.HR.Domain.Repositories.Pay;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Ksc.HR.Data.Persistant.Repositories.Pay
{

    public class OtherPaymentSettingParameterRepository : EfRepository<OtherPaymentSettingParameter, int>, IOtherPaymentSettingParameterRepository
    {

        private readonly KscHrContext _kscHrContext;
        public OtherPaymentSettingParameterRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }


        public IQueryable<OtherPaymentSettingParameter> GetOtherPaymentSettingParameters()
        {
            var result = _kscHrContext.OtherPaymentSettingParameters.Include(x => x.OtherPaymentSettingParameterValues).AsQueryable();
            return result;
        }

    }
}

using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Pay;
using Ksc.HR.Domain.Repositories.Pay;
using KSC.Infrastructure.Persistance;

namespace Ksc.HR.Data.Persistant.Repositories.Pay
{

    public class OtherPaymentPercentageRepository : EfRepository<OtherPaymentPercentage, int>, IOtherPaymentPercentageRepository
    {

        private readonly KscHrContext _kscHrContext;
        public OtherPaymentPercentageRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

    }
}

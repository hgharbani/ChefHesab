using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Pay;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Repositories.Pay;
using KSC.Infrastructure.Persistance;
using System.Linq.Dynamic.Core;

namespace Ksc.HR.Data.Persistant.Repositories.Pay
{

    public class PaymentProcessCategoryRepository : EfRepository<PaymentProcessCategory, int>, IPaymentProcessCategoryRepository
    {

        private readonly KscHrContext _kscHrContext;
        public PaymentProcessCategoryRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
    }
}

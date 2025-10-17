using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Pay;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Repositories.Pay;
using Ksc.HR.Share.Model.OtherPaymentStatus;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Ksc.HR.Data.Persistant.Repositories.Pay
{

    public class PaymentDetailRepository : EfRepository<PaymentDetail, long>, IPaymentDetailRepository
    {

        private readonly KscHrContext _kscHrContext;
        public PaymentDetailRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<PaymentDetail> GetAllIncluded()
        {
            var query = GetAllQueryable().Include(a => a.Employee).Include(a => a.AccountCode);
            return query;
        }
        public IQueryable<PaymentDetail> GetPaymentDetailForAccountingSystemByHeaderId(int paymentHeaderId)
        {
            var query = _kscHrContext.PaymentDetails.Where(x => x.PaymentHeaderId== paymentHeaderId).Include(x => x.AccountCode).Include(x=>x.Employee);
            return query;
        }
        
    }
}

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

    public class PaymentHeaderRepository : EfRepository<PaymentHeader, int>, IPaymentHeaderRepository
    {

        private readonly KscHrContext _kscHrContext;
        public PaymentHeaderRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<PaymentHeader> GetPaymentHeaderForAccountingSystem(List<int> paymentProcessIds)
        {
            var query = _kscHrContext.PaymentHeaders.Where(x =>
            paymentProcessIds.Any(i => i == x.PaymentProcessId) && x.OtherPaymentStatusId == EnumOtherPaymentStatus.ConfirmAndSendToAccounting.Id
            && x.AccountingDocumentNumber == null).Include(x=>x.PaymentProcess);
            return query;
        }
        public IQueryable<PaymentHeader> GetAllIncluded()
        {
            var query = GetAllQueryable().Include(a=>a.PaymentProcess).Include(a=>a.OtherPaymentStatus);
            return query;
        }

    }
}

using Ksc.Hr.Domain.Entities;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Chart;
using Ksc.HR.Domain.Entities.Salary;
using Ksc.HR.Domain.Repositories.Salary;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Ksc.HR.Data.Persistant.Repositories.Salary
{
    public class PaymentAccountCodeRepository : EfRepository<PaymentAccountCode, int>, IPaymentAccountCodeRepository
    {

        private readonly KscHrContext _kscHrContext;
        public PaymentAccountCodeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<PaymentAccountCode> GetPaymentAccountCodeById(int id)
        {
            return _kscHrContext.PaymentAccountCodes.Where(a => a.Id == id);
        }
        public IQueryable<PaymentAccountCode> GetPaymentAccountCodes()
        {
            var result = _kscHrContext.PaymentAccountCodes.AsQueryable();
            return result;
        }

        public IQueryable<PaymentAccountCode> GetAllInclude()
        {
            return _kscHrContext.PaymentAccountCodes
                .AsQueryable();
        }
    }
}

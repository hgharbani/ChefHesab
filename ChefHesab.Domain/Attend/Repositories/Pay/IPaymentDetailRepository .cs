using KSC.Domain;
using Ksc.HR.Domain.Entities.Pay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.Pay
{
    public interface IPaymentDetailRepository : IRepository<PaymentDetail, long>
    {
        IQueryable<PaymentDetail> GetAllIncluded();
        IQueryable<PaymentDetail> GetPaymentDetailForAccountingSystemByHeaderId(int paymentHeaderId);
    }
}

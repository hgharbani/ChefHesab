using Ksc.HR.Domain.Entities.Pay;
using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.Pay
{
    public interface IOtherPaymentHeaderTypeRepository : IRepository<OtherPaymentHeaderType, int>
    {
        IQueryable<OtherPaymentHeaderType> GetOtherPaymentHeaderTypeByHeaderId(int headerId);
    }
}

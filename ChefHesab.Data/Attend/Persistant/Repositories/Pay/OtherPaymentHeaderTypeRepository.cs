using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Pay;
using Ksc.HR.Domain.Repositories.Pay;
using KSC.Infrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.Pay
{
    public class OtherPaymentHeaderTypeRepository : EfRepository<OtherPaymentHeaderType, int>, IOtherPaymentHeaderTypeRepository
    {
        private readonly KscHrContext _kscHrContext;
        public OtherPaymentHeaderTypeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<OtherPaymentHeaderType> GetOtherPaymentHeaderTypeByHeaderId(int headerId)
        {
            return _kscHrContext.OtherPaymentHeaderTypes.Where(x => x.OtherPaymentHeaderId == headerId);

        }
    }
}

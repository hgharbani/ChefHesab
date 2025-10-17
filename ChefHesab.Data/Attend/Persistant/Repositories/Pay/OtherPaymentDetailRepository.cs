using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Pay;
using Ksc.HR.Domain.Repositories.Pay;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.Pay
{
    public class OtherPaymentDetailRepository : EfRepository<OtherPaymentDetail, int>, IOtherPaymentDetailRepository
    {
        private readonly KscHrContext _kscHrContext;
        public OtherPaymentDetailRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<OtherPaymentDetail> GetOtherPaymentDetailByHeaderId(int headerId)
        {
            return _kscHrContext.OtherPaymentDetail.AsQueryable().Include(x=>x.OtherPaymentStatus).Where(x=>x.OtherPaymentHeaderId == headerId);
        }
    }
}

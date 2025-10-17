using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Pay;
using Ksc.HR.Domain.Entities.Salary;
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
    public class OtherPaymentStatusFlowRepository : EfRepository<OtherPaymentStatusFlow, int>, IOtherPaymentStatusFlowRepository
    {
        private readonly KscHrContext _kscHrContext;

        public OtherPaymentStatusFlowRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<OtherPaymentStatusFlow> GetAllIncludesOtherPaymentStatusFlow()
        {
            var result = _kscHrContext.OtherPaymentStatusFlow.Include(a => a.CurrentStatus).Include(a => a.NextStatus).AsQueryable();
            return result;
        }
    }
}

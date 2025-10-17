using Ksc.Hr.Domain.Entities;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Chart;
using Ksc.HR.Domain.Repositories.Chart;
using Ksc.HR.Domain.Repositories.EmployeeBase;
using KSC.Infrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.EmployeeBase
{
    public class PaymentStatusAccessRepository : EfRepository<PaymentStatusAccess, int>, IPaymentStatusAccessRepository
    {
        private readonly KscHrContext _kscHrContext;
        public PaymentStatusAccessRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<PaymentStatusAccess> GetPaymentStatusAccessById(int id)
        {
            return _kscHrContext.PaymentStatusAccesses.Where(a => a.Id == id);
        }
        public IQueryable<PaymentStatusAccess> GetPaymentStatusAccesses()
        {
            var result = _kscHrContext.PaymentStatusAccesses.AsQueryable();
            return result;
        }
        public IQueryable<PaymentStatusAccess> GetPaymentStatusAccessesByRoles(List<string> roles)
        {
         var result= _kscHrContext.PaymentStatusAccesses.Where(a => roles.Contains(a.AccessLevel.RoleInIdentityService));
            return result;
        }
    }
}

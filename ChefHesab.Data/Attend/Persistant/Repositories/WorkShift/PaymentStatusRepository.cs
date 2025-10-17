using KSC.Infrastructure.Persistance;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Workshift;
using Ksc.HR.Domain.Repositories.WorkShift;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.WorkShift
{
    public class PaymentStatusRepository : EfRepository<PaymentStatus, int>, IPaymentStatusRepository
    {
        private readonly KscHrContext _kscHrContext;
        public PaymentStatusRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext= KscHrContext;
        }
        public IQueryable<PaymentStatus> GetAllPaymentStatusNoTracking(int id)
        {
            return _kscHrContext.PaymentStatus.Where(a => a.Id == id);
        }
        public IQueryable<PaymentStatus> GetAllPaymentStatusNoTrackingWithIds(List<int> ids)
        {
            return _kscHrContext.PaymentStatus.Where(a => ids.Contains(a.Id));
        }
    }
}

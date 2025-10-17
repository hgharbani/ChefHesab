using KSC.Domain;
using Ksc.HR.Domain.Entities.Workshift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.WorkShift
{
    public interface IPaymentStatusRepository : IRepository<PaymentStatus, int>
    {
        IQueryable<PaymentStatus> GetAllPaymentStatusNoTracking(int id);
        IQueryable<PaymentStatus> GetAllPaymentStatusNoTrackingWithIds(List<int> ids);
    }
}

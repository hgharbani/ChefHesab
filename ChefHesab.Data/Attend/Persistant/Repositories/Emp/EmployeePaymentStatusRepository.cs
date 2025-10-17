using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Emp;
using Ksc.HR.Domain.Entities.EmployeeBase;
using Ksc.HR.Domain.Repositories.Emp;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Ksc.HR.Data.Persistant.Repositories.Emp
{
    public class EmployeePaymentStatusRepository : EfRepository<EmployeePaymentStatus, int>, IEmployeePaymentStatusRepository
    {
        private readonly KscHrContext _kscHrContext;
        public EmployeePaymentStatusRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<EmployeePaymentStatus> GetAllEmployeePaymentStatusByRelated(int? id)
        {
            var result = _kscHrContext.EmployeePaymentStatues.Include(x => x.PaymentStatus)
                         .Where(x => x.EmployeeId == id).AsQueryable().AsNoTracking();
            return result;
        }
        //public IQueryable<EmployeePaymentStatus> GetAllEmployeePaymentStatusNoTracking(int id)
        //{
        //    return _kscHrContext.EmployeePaymentStatues.Where(a => a.Id == id);
        //}
    }
}

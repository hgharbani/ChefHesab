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
    public class EmployeeOtherPaymentHistoryRepository : EfRepository<EmployeeOtherPaymentHistory, long>, IEmployeeOtherPaymentHistoryRepository
    {
        private readonly KscHrContext _kscHrContext;
        public EmployeeOtherPaymentHistoryRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<EmployeeOtherPaymentHistory> GetEmployeeOtherPaymentHistoryByEmployeeOtherPaymentId(int employeeOtherPaymentId)
        {
            var query = _kscHrContext.EmployeeOtherPaymentHistory.Where(x => x.EmployeeOtherPaymentId == employeeOtherPaymentId);
            return query;
        }
        
    }
}

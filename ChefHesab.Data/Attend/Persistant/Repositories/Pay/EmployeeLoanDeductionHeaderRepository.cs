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
    public class EmployeeLoanDeductionHeaderRepository : EfRepository<EmployeeLoanDeductionHeader, int>, IEmployeeLoanDeductionHeaderRepository
    {
        private readonly KscHrContext _kscHrContext;
        public EmployeeLoanDeductionHeaderRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<EmployeeLoanDeductionHeader> GetEmployeeLoanDeductionHeaderIncluded()
        {
            var query = _kscHrContext.EmployeeLoanDeductionHeader
                .Include(a => a.PaymentAccountCode)
                .Include(a => a.InstallmentAccountCode)
               // .Include(a=>a.EmployeeLoanDeductionDetails)
                ;
            return query;
        }
    }
}

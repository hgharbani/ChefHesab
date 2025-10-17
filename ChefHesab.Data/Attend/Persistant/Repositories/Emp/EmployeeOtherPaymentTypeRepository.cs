using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities;
using Ksc.HR.Domain.Entities.Emp;
using Ksc.HR.Domain.Entities.Pay;
using Ksc.HR.Domain.Repositories.Emp;
using Ksc.HR.Domain.Repositories.EmployeeBase;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.Emp
{
    public class EmployeeOtherPaymentTypeRepository : EfRepository<EmployeeOtherPaymentType, long>, IEmployeeOtherPaymentTypeRepository
    {
        private readonly KscHrContext _kscHrContext;
        public EmployeeOtherPaymentTypeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<EmployeeOtherPaymentType> GetAllByYear(int? year)
        {
            var result = _kscHrContext.EmployeeOtherPaymentType.Include(x => x.EmployeeOtherPayment)
                         .Where(x => x.Year == year && !x.EmployeeOtherPayment.IsBlacklist).AsQueryable();
            return result;
        }
    }
}

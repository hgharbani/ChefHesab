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
    public class EmployeeDeductionTypeRepository : EfRepository<EmployeeDeductionType, int>, IEmployeeDeductionTypeRepository
    {

        private readonly KscHrContext _kscHrContext;
        public EmployeeDeductionTypeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        /// <summary>
        /// با انتخاب نوع بدهی کد حساب پرداخت و شرکت اعتبار دهنده را پیدا میکند
        /// </summary>
        public EmployeeDeductionType GetByIdWithIncludedAccountCode(int id,bool asNoTraking = false)
        {
            var query = _kscHrContext.EmployeeDeductionType.Where(x => x.Id == id);
            if (asNoTraking)
            {
                query = query.AsNoTracking();
            }

            var result = query.Include(x => x.AccountCodeDeductionTypes.Where(p => p.IsActive)) // یافتن کد حساب
                .ThenInclude(x => x.AccountCode)
                .ThenInclude(x => x.AccountCodeBeneficiaries.Where(p => p.IsActive)) // یافتن کد شرکت اعتباردهنده
                .AsQueryable()
                .FirstOrDefault();

            return result;
        }
    }
}

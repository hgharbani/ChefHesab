using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Pay;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Repositories.Pay;
using Ksc.HR.DTO.MIS;
using Ksc.HR.Share.Model.DeductionAdditional;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.Pay
{
    public class EmployeeDeductionAdditionalRepository : EfRepository<EmployeeDeductionAdditional, int>, IEmployeeDeductionAdditionalRepository
    {
        private readonly KscHrContext _kscHrContext;
        public EmployeeDeductionAdditionalRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<EmployeeDeductionAdditional> GetByDeductionAdditionalId(int deductionAdditionalId)
        {
            var result = _kscHrContext.EmployeeDeductionAdditionals.Where(x => x.DeductionAdditionalId == deductionAdditionalId)
                .Include(x => x.Employee).ThenInclude(x => x.EmploymentType) //پرسنل
                .Include(x=>x.Pay_DeductionAdditional).ThenInclude(x=>x.Salary_AccountCode)
                ;

            return result;
        }


        public IQueryable<EmployeeDeductionAdditional> GetDeductionAdditionals()
        {
            var result = _kscHrContext.EmployeeDeductionAdditionals.AsNoTracking()
                
                .Include(x => x.Employee).ThenInclude(x => x.EmploymentType) //پرسنل
                .Include(x => x.Pay_DeductionAdditional).ThenInclude(x => x.Salary_AccountCode)
                ;

            return result;
        }
    }
}

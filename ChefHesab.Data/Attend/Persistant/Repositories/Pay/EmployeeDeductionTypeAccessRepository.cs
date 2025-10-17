using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Pay;
using Ksc.HR.Domain.Repositories.Pay;
using KSC.Infrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.Pay
{
    public class EmployeeDeductionTypeAccessRepository : EfRepository<EmployeeDeductionTypeAccess, int>, IEmployeeDeductionTypeAccessRepository
    {

        private readonly KscHrContext _kscHrContext;
        public EmployeeDeductionTypeAccessRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<EmployeeDeductionTypeAccess> GetEmployeeDeductionTypeAccessByRoles(List<string> roles)
        {
            var result = _kscHrContext.EmployeeDeductionTypeAccess.Where(a => a.IsActive == true && roles.Contains(a.RoleName));
            return result;
        }
    }
}

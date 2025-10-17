using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Rule;
using Ksc.HR.Domain.Entities.Salary;
using Ksc.HR.Domain.Repositories.Rule;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.Salary
{
    public class AccountCodeMaritalRepository : EfRepository<AccountCodeMarital, int>, IAccountCodeMaritalRepository
    {
        private readonly KscHrContext _kscHrContext;
        public AccountCodeMaritalRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<AccountCodeMarital> GetAllIncluded()
        {
            var result = _kscHrContext.AccountCodeMarital.Include(x => x.AccountCode).Include(x => x.MaritalStatus).Where(x => x.IsActive == true).AsQueryable();
            return result;
        }
        public IQueryable<AccountCodeMarital> GetAccountCodeMaritals(int accountcodeId)
        {
            var result = _kscHrContext.AccountCodeMarital.Include(x => x.AccountCode).Include(x => x.MaritalStatus).Where(x => x.AccountCodeId == accountcodeId && x.IsActive == true).AsQueryable();
            return result;
        }
    }
}

using Ksc.Hr.Domain.Entities;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Salary;
using Ksc.HR.Domain.Repositories.Salary;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Ksc.HR.Data.Persistant.Repositories.Salary
{
    public class AccountCodeCompatibleRepository : EfRepository<AccountCodeCompatible, int>, IAccountCodeCompatibleRepository
    {

        private readonly KscHrContext _kscHrContext;
        public AccountCodeCompatibleRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<AccountCodeCompatible> GetAccountCodeCompatibleById(int id)
        {
            return _kscHrContext.AccountCodeCompatibles.Where(a => a.Id == id);
        }
        public IQueryable<AccountCodeCompatible> GetAccountCodeCompatibles()
        {
            var result = _kscHrContext.AccountCodeCompatibles.AsQueryable();
            return result;
        }

        public IQueryable<AccountCodeCompatible> GetAllInclude()
        {
            return _kscHrContext.AccountCodeCompatibles
                .AsQueryable().Include(x=>x.AccountCodeCompatibleType);
        }
    }
}

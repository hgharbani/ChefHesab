using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Pay;
using Ksc.HR.Domain.Entities.Salary;
using Ksc.HR.Domain.Repositories.Salary;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Ksc.HR.Data.Persistant.Repositories.Salary
{
    public  class PropertyAccountRepository : EfRepository<PropertyAccount, int>, IPropertyAccountRepository
    {

        private readonly KscHrContext _kscHrContext;
        public PropertyAccountRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<PropertyAccount> GetPropertyAccountWithTypes(int typeId)
        {
            var result = _kscHrContext.PropertyAccounts.Include(x => x.PropertyAccountType).Where(x=>x.PropertyAccountTypeId==typeId).AsQueryable();
            return result;
        }

    }
}

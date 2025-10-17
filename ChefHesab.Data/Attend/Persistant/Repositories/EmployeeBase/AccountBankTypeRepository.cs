using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.EmployeeBase;
using Ksc.HR.Domain.Repositories.EmployeeBase;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Ksc.HR.Data.Persistant.Repositories.EmployeeBase
{
    public class AccountBankTypeRepository : EfRepository<AccountBankType, int>, IAccountBankTypeRepository
    {
        private readonly KscHrContext _kscHrContext;
        public AccountBankTypeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<AccountBankType> GetAllAccountBankTypeNoTracking(int id)
        {
            return _kscHrContext.AccountBankTypes.Where(a => a.Id == id);
        }

        IQueryable<AccountBankType> IAccountBankTypeRepository.GetAll()
        {
            return _kscHrContext.AccountBankTypes
                .IgnoreAutoIncludes();
        }
    }
}

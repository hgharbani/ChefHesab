using Ksc.Hr.Domain.Entities;
using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Salary;
using Ksc.HR.Domain.Repositories.Salary;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System.Xml.Schema;

namespace Ksc.HR.Data.Persistant.Repositories.Salary
{
    public class AccountCodeRepository : EfRepository<AccountCode, int>, IAccountCodeRepository
    {

        private readonly KscHrContext _kscHrContext;
        public AccountCodeRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<AccountCode> GetAccountCodeById(int id)
        {
            return _kscHrContext.AccountCode.Where(a => a.Id == id);
        }
        public IQueryable<AccountCode> GetAccountCodes()
        {
            var result = _kscHrContext.AccountCode.AsQueryable();
            return result;
        }

        public IQueryable<AccountCode> GetAllRelated()
        {
            var result = _kscHrContext.AccountCode.AsQueryable()
                .Include(x => x.AccountCodeCategory)
                 .Include(x => x.AccountCodeCompatibles_AccountCodeId).ThenInclude(x => x.Salary_AccountCodeCompatible)

                .Include(x => x.AccountCodeCompatibles_AccountCodeId).ThenInclude(x => x.AccountCodeCompatibleType)
                .Include(x => x.PropertyAccountSettings).ThenInclude(x => x.PropertyAccount).ThenInclude(x => x.PropertyAccountType)
                .Include(x => x.PaymentAccountCodes)//.ThenInclude(x=>x.PaymentType)
                .Include(x => x.Salary_AccountCodeMaritals)//.ThenInclude(x=>x.)
                .Include(x => x.AccountEmploymentTypes)
                .Include(x => x.AccountCodeBeneficiaries)
                .Include(x => x.AccountCodeDeductionTypes)
                ;
            return result;
        }
        public IQueryable<AccountCode> GetIndexGridRelated()
        {
            var result = _kscHrContext.AccountCode.AsQueryable()
                .Include(x => x.AccountCodeCategory)
                .Include(x => x.InterdictCategory)
               .Include(x => x.PropertyAccountSettings)
                .ThenInclude(x => x.PropertyAccount);
            return result;
        }
        public IQueryable<AccountCode> GetActiveAccountCodes()
        {
            var result = GetAccountCodes().Where(x => x.IsActive);
            return result;
        }

        public IQueryable<AccountCode> GetForOtherFileAdditional()
        {
            var result = _kscHrContext.AccountCode
                .Include(x => x.PropertyAccountSettings)
                .ThenInclude(x => x.PropertyAccount)
                .Where(x => x.PropertyAccountSettings.Any(c => c.PropertyAccountId == 21))//یک ساعت اضافه کاری
                .AsQueryable();
            return result;
        }
    }
}

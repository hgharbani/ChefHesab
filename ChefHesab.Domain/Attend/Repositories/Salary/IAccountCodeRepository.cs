using Ksc.Hr.Domain.Entities;
using Ksc.HR.Domain.Entities.Salary;
using KSC.Domain;

namespace Ksc.HR.Domain.Repositories.Salary
{
    public interface IAccountCodeRepository : IRepository<AccountCode, int>
    {
         IQueryable<AccountCode> GetAccountCodeById(int id);
         IQueryable<AccountCode> GetAccountCodes();
        IQueryable<AccountCode> GetActiveAccountCodes();
        IQueryable<AccountCode> GetAllRelated();
        IQueryable<AccountCode> GetForOtherFileAdditional();
        IQueryable<AccountCode> GetIndexGridRelated();
    }
}

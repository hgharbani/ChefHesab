using Ksc.Hr.Domain.Entities;
using Ksc.HR.Domain.Entities.Salary;
using KSC.Domain;

namespace Ksc.HR.Domain.Repositories.Salary
{
    public interface IAccountCodeCompatibleRepository : IRepository<AccountCodeCompatible, int>
    {
        IQueryable<AccountCodeCompatible> GetAccountCodeCompatibleById(int id);
        IQueryable<AccountCodeCompatible> GetAccountCodeCompatibles();
        IQueryable<AccountCodeCompatible> GetAllInclude();
    }
}

using Ksc.Hr.Domain.Entities;
using Ksc.HR.Domain.Entities.Salary;
using KSC.Domain;

namespace Ksc.HR.Domain.Repositories.Salary
{
    public interface IAccountCodeCategoryRepository : IRepository<AccountCodeCategory, int>
    {
        IQueryable<AccountCodeCategory> GetAccountCodeCategoryById(int id);
        IQueryable<AccountCodeCategory> GetAccountCodeCategories();
        IQueryable<AccountCodeCategory> GetAllInclude();
    }
}

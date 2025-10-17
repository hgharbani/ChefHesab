using Ksc.HR.Domain.Entities.Salary;
using KSC.Domain;

namespace Ksc.HR.Domain.Repositories.Salary
{
    public interface IPropertyAccountRepository : IRepository<PropertyAccount, int>
    {
        IQueryable<PropertyAccount> GetPropertyAccountWithTypes(int typeId);
       // IQueryable<PropertyAccount> GetPropertyAccountWithTypes();
    }
}

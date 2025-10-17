using Ksc.Hr.Domain.Entities;
using Ksc.HR.Domain.Entities.Salary;
using KSC.Domain;

namespace Ksc.HR.Domain.Repositories.Salary
{
    public interface IAccountCodeDeductionTypeRepository : IRepository<AccountCodeDeductionType, int>
    {
        IQueryable<AccountCodeDeductionType> GetAllInclude();
        AccountCodeDeductionType GetDeductonTypeByAccountCode(int id);
    }
}

using KSC.Domain;
using Ksc.HR.Domain.Entities.Salary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Domain.Entities.Rule;

namespace Ksc.HR.Domain.Repositories.Rule
{
    public interface IAccountEmploymentTypeRepository : IRepository<AccountEmploymentType, int>
    {
        IQueryable<AccountEmploymentType> GetAccountEmploymeeWithEmpType(int? employeeId, int? empTypeId);
        IQueryable<AccountEmploymentType> GetAccountEmploymee(int? employeeId);
        IQueryable<AccountEmploymentType> GetAccountEmploymentTypes(int? employmentTypeId);
        IQueryable<AccountEmploymentType> GetActive_IsGroupSalaryAmount_AccountEmploymentTypes(int? employmentTypeId);
        IQueryable<AccountEmploymentType> GetAllIncludesAccount();
    }
}

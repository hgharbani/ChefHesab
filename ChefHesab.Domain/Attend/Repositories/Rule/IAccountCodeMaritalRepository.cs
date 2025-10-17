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
    public interface IAccountCodeMaritalRepository : IRepository<AccountCodeMarital, int>
    {
        IQueryable<AccountCodeMarital> GetAccountCodeMaritals(int accountcodeId);
        IQueryable<AccountCodeMarital> GetAllIncluded();
    }
}

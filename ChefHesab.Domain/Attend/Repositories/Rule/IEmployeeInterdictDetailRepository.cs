using KSC.Domain;
using Ksc.HR.Domain.Entities.Rule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.Rule
{
    public interface IEmployeeInterdictDetailRepository : IRepository<EmployeeInterdictDetail, int>
    {
        IQueryable<EmployeeInterdictDetail> GetInterdictDetailByInterdictId(int id);
        IQueryable<EmployeeInterdictDetail> GetInterdictDetailByInterdictIds(int[] ids);
    }
}

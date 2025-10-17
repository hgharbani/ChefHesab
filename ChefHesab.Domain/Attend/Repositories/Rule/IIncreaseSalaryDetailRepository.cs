using KSC.Domain;
using Ksc.HR.Domain.Entities.Rule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.Rule
{
    public interface IIncreaseSalaryDetailRepository : IRepository<IncreaseSalaryDetail, int>
    {
        IQueryable<IncreaseSalaryDetail> GetIncreaseSalaryDetailByRelated(int? year);
        IQueryable<IncreaseSalaryDetail> GetIncreaseSalaryDetailByHeader(int headerId);
        void UpdateRange(List<IncreaseSalaryDetail> ncreaseSalary);
    }
}

using KSC.Domain;
using Ksc.HR.Domain.Entities.Rule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.Rule
{
    public interface IEmployeePercentMeritHistoryRepository : IRepository<EmployeePercentMeritHistory, int>
    {
        Task<bool> AddBulkAsync(List<EmployeePercentMeritHistory> list);
        IQueryable<EmployeePercentMeritHistory> EmployeePercentMeritHistoryByYear(int? year);
        void RemoveEmployeePercentMeritHistoryByYear(int? year);
    }
}

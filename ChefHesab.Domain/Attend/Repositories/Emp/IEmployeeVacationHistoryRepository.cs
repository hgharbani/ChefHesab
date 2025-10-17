using Ksc.HR.Domain.Entities;

using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.Emp
{
    public interface IEmployeeVacationHistoryRepository : IRepository<EmployeeVacationHistory, int>
    {
        Task<bool> AddBulkAsync(List<EmployeeVacationHistory> list);
        Task<bool> BulkDeleteAsync(List<EmployeeVacationHistory> list);
        Task<bool> BulkInsertOrUpdateAsync(List<EmployeeVacationHistory> list);
        Task<bool> BulkUpdateAsync(EmployeeVacationHistory entity);
        Task<bool> BulkUpdateAsync(List<EmployeeVacationHistory> list);
        IQueryable<EmployeeVacationHistory> GetAllRelated();

        IQueryable<EmployeeVacationHistory> GetByIdRelated(int id);
        IQueryable<EmployeeVacationHistory> GetByRelated();
        IQueryable<EmployeeVacationHistory> GetLatestData(int yearMonth);
    }
}

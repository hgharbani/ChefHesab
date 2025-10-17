using Ksc.Hr.Domain.Entities;
 using KSC.Domain;

namespace Ksc.Hr.Domain.Repositories
  {
    public interface IEmployeeHaveTravelRepository : IRepository<EmployeeHaveTravel, long>
    {
        Task<bool> AddBulkAsync(List<EmployeeHaveTravel> list);
        Task<bool> AddBulkAsync(EmployeeHaveTravel entity);
        Task<bool> SaveBulkAsync();
        Task<bool> UpdateBulkAsync(List<EmployeeVacationManagement> list);
    }
}


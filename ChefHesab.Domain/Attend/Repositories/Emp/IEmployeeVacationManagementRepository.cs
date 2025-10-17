using Ksc.Hr.Domain.Entities;
using Ksc.HR.Domain.Entities;
using KSC.Domain;

namespace Ksc.Hr.Domain.Repositories
  {
    public interface IEmployeeVacationManagementRepository : IRepository<EmployeeVacationManagement, long>
    {
        Task<bool> AddBulkAsync(EmployeeVacationManagement entity);
        Task<bool> AddBulkAsync(List<EmployeeVacationManagement> list);
        IQueryable<EmployeeVacationManagement> GetAllRelated();
        IQueryable<EmployeeVacationManagement> GetByEmployeeId(int employeeId);
        IQueryable<EmployeeVacationManagement> GetByIdRelated(int id);
        Task<bool> SaveBulkAsync();
        Task<bool> UpdateBulkAsync(EmployeeVacationManagement entity);
        Task<bool> UpdateBulkAsync(List<EmployeeVacationManagement> list);
        Task<bool> UpdateBulkWithoutSaveAsync(List<EmployeeVacationManagement> list);
        void UpdateCurrentYearMerit(int employeeId, double duration, string remark,string userName);
        void UpdateLastYearVacationRemaining(int employeeId, double duration, string remark, string userName, bool isSurplusVacation = false);
        void UpdateRange(List<EmployeeVacationManagement> list);
        void UpdateUsedCurrentYear(int employeeId, double duration, string remark, string userName);
    }
}


using Ksc.Hr.Domain.Entities;
using Ksc.HR.Domain.Entities;
using KSC.Domain;

namespace Ksc.Hr.Domain.Repositories
  {
    public interface IEmployeeVacationSoldRepository : IRepository<EmployeeVacationSold, int>
    {
        IQueryable<EmployeeVacationSold> GetAllRelated();
        IQueryable<EmployeeVacationSold> GetByIdRelated(int id);

    }
}


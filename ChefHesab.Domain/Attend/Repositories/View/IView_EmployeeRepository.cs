
using Ksc.HR.Domain.Entities.View;
using KSC.Domain;
namespace Ksc.Hr.Domain.Repositories
{
    public interface IView_EmployeeRepository : IRepository<View_Employee>
    {
        Task<View_Employee> GetEmployeesByWinUser(string userName);
    }
}


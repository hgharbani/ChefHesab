using Ksc.HR.Domain.Entities.Emp;

using KSC.Domain;


namespace Ksc.HR.Domain.Repositories.Emp
{
    public interface IEmployeePaymentStatusRepository : IRepository<EmployeePaymentStatus, int>
    {
        IQueryable<EmployeePaymentStatus> GetAllEmployeePaymentStatusByRelated(int? id);
    }
}

using Ksc.Hr.Domain.Entities;
using KSC.Domain;

namespace Ksc.HR.Domain.Repositories.EmployeeBase
{
    public interface IPaymentStatusAccessRepository : IRepository<PaymentStatusAccess, int>
    {
        IQueryable<PaymentStatusAccess> GetPaymentStatusAccessById(int id);
        IQueryable<PaymentStatusAccess> GetPaymentStatusAccesses();
        IQueryable<PaymentStatusAccess> GetPaymentStatusAccessesByRoles(List<string> roles);
    }
}

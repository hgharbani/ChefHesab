using Ksc.Hr.Domain.Entities;
using Ksc.HR.Domain.Entities.Salary;
using KSC.Domain;

namespace Ksc.HR.Domain.Repositories.Salary
{
    public interface IPaymentAccountCodeRepository : IRepository<PaymentAccountCode, int>
    {
        IQueryable<PaymentAccountCode> GetPaymentAccountCodeById(int id);
        IQueryable<PaymentAccountCode> GetPaymentAccountCodes();
        IQueryable<PaymentAccountCode> GetAllInclude();
    }
}

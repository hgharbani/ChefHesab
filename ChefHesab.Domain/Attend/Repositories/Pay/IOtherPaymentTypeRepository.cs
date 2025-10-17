using Ksc.HR.Domain.Entities.Pay;
using KSC.Domain;

namespace Ksc.HR.Domain.Repositories.Pay
{
    public interface IOtherPaymentTypeRepository : IRepository<OtherPaymentType, int>
    {
        IQueryable<OtherPaymentType> GetOtherPaymentTypeById(int id);
        IQueryable<OtherPaymentType> GetOtherPaymentTypes();
    }
}

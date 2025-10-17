using Ksc.HR.Domain.Entities.Pay;
using KSC.Domain;

namespace Ksc.HR.Domain.Repositories.Pay
{
    public interface IOtherPaymentSettingParameterRepository : IRepository<OtherPaymentSettingParameter, int>
    {
        IQueryable<OtherPaymentSettingParameter> GetOtherPaymentSettingParameters();
    }
}

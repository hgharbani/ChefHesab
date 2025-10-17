using Ksc.HR.Domain.Entities.Pay;
using KSC.Domain;

namespace Ksc.HR.Domain.Repositories.Pay
{
    public interface IOtherPaymentSettingParameterValueRepository : IRepository<OtherPaymentSettingParameterValue, int>
    {
        IQueryable<OtherPaymentSettingParameterValue> GetAllQueryableBySettingId(int settingId);
    }
}

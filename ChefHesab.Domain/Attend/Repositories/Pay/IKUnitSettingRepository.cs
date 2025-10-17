using Ksc.HR.Domain.Entities.Pay;
using KSC.Domain;

namespace Ksc.HR.Domain.Repositories.Pay
{
    public interface IKUnitSettingRepository : IRepository<KUnitSetting, int>
    {
        long? GetKUnitByYear(int salaryDate);
    }
}

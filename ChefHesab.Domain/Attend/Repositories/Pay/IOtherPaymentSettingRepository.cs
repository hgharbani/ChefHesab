using Ksc.HR.Domain.Entities.Pay;
using KSC.Domain;

namespace Ksc.HR.Domain.Repositories.Pay
{
    public interface IOtherPaymentSettingRepository : IRepository<OtherPaymentSetting, int>
    {
        IQueryable<OtherPaymentSetting> GetAllByFilterYearMonthAccountCode(int? yearMonthStartReport, int? yearMonthEndReport, int accountCodeId);
        IQueryable<OtherPaymentSetting> GetIncludeOtherPaymentType();
        IQueryable<OtherPaymentSetting> GetOtherPaymentSettingById(int id);
        OtherPaymentSetting GetOtherPaymentSettingByotherPaymentTypeForTravel(int startDate, int endDate, int otherPaymentType);
        IQueryable<OtherPaymentSetting> GetOtherPaymentSettings();
        bool GetPermitSettingByFilter(int startDate, int endDate, int otherPaymentType);
        //bool IsExistActiveSetting(int? yearMonthStartReport, int? yearMonthEndReport, int accountCodeId);
    }
}

using KSC.Domain;
using Ksc.HR.Domain.Entities.Pay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Repositories.Pay
{
    public interface IPaymentAdditionalSettingRepository : IRepository<PaymentAdditionalSetting, int>
    {
        IQueryable<PaymentAdditionalSetting> GetAllQueryableByDate_AccountId(int yearMonth, int accountCodeId);
        IQueryable<PaymentAdditionalSetting> GetAllQueryableByDate_AccountIdWithOutInclude(int yearMonth, int accountCodeId);
        IQueryable<PaymentAdditionalSetting> GetJobPosition_AccountSettingId(int yearMonth, int accountCodeId);
    }
}

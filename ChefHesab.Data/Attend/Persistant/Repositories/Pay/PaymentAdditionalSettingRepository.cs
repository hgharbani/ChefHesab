using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Pay;
using Ksc.HR.Domain.Repositories.Pay;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Data.Persistant.Repositories.Pay
{
    public class PaymentAdditionalSettingRepository : EfRepository<PaymentAdditionalSetting, int>, IPaymentAdditionalSettingRepository
    {

        private readonly KscHrContext _kscHrContext;
        public PaymentAdditionalSettingRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public IQueryable<PaymentAdditionalSetting> GetJobPosition_AccountSettingId(int yearMonth, int accountCodeId)
        {
            var query = GetAllQueryable()
                .Where(a => a.YearMonthStart <= yearMonth && a.YearMonthEnd >= yearMonth &&
                a.IsActive &&
                a.AccountCodeId == accountCodeId)
                .Include(a => a.PaymentAdditionalSettingJobPositions);
            return query;
        }

        public IQueryable<PaymentAdditionalSetting> GetAllQueryableByDate_AccountId(int yearMonth, int accountCodeId)
        {
            var query = GetAllQueryable()
                .Where(a => a.YearMonthStart <= yearMonth && a.YearMonthEnd >= yearMonth &&
                a.IsActive &&
                a.AccountCodeId == accountCodeId)
                .Include(a => a.PaymentAdditionalSettingJobCategories)
                .Include(a => a.PaymentAdditionalSettingWorkCities);
            return query;
        }

        public IQueryable<PaymentAdditionalSetting> GetAllQueryableByDate_AccountIdWithOutInclude(int yearMonth, int accountCodeId)
        {
            var query = GetAllQueryable()
                .Where(a => a.YearMonthStart <= yearMonth && a.YearMonthEnd >= yearMonth &&
                a.IsActive &&
                a.AccountCodeId == accountCodeId)
                //.Include(a => a.PaymentAdditionalSettingJobCategories)
                //.Include(a => a.PaymentAdditionalSettingWorkCities)
                ;
            return query;
        }
    }
}

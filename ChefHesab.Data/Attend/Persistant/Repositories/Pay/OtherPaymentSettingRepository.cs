using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Pay;
using Ksc.HR.Domain.Entities.Salary;
using Ksc.HR.Domain.Repositories.Pay;
using KSC.Common.Filters.Models;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;


namespace Ksc.HR.Data.Persistant.Repositories.Pay
{

    public class OtherPaymentSettingRepository : EfRepository<OtherPaymentSetting, int>, IOtherPaymentSettingRepository
    {

        private readonly KscHrContext _kscHrContext;
        public OtherPaymentSettingRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }

        public IQueryable<OtherPaymentSetting> GetOtherPaymentSettingById(int id)
        {
            return _kscHrContext.OtherPaymentSetting.Where(a => a.Id == id);
        }
        public IQueryable<OtherPaymentSetting> GetOtherPaymentSettings()
        {
            var result = _kscHrContext.OtherPaymentSetting.Include(x => x.OtherPaymentType).Include(x => x.AccountCode).AsQueryable();
            return result;
        }
        public IQueryable<OtherPaymentSetting> GetIncludeOtherPaymentType()
        {
            var result = _kscHrContext.OtherPaymentSetting.Where(x => x.IsActive).Include(x => x.OtherPaymentType);
            return result;
        }
        public IQueryable<OtherPaymentSetting> GetAllByFilterYearMonthAccountCode(int? yearMonthStartReport, int? yearMonthEndReport, int accountCodeId)
        {
            var result = GetIncludeOtherPaymentType().Where(x =>
            x.IsActive == true &&
            x.ValidityStartYearMonth <= yearMonthStartReport
            && (x.ValidityEndYearMonth >= yearMonthEndReport || x.ValidityEndYearMonth.HasValue == false)
            && x.AccountCodeId == accountCodeId
            );
            return result;
        }
        public bool GetPermitSettingByFilter(int startDate, int endDate, int otherPaymentType)
        {

            var isExist = GetOtherPaymentSettingByotherPaymentTypeForTravel(startDate, endDate, otherPaymentType) != null;
            return isExist;
        }
        public OtherPaymentSetting GetOtherPaymentSettingByotherPaymentTypeForTravel(int startDate, int endDate, int otherPaymentType)
        {

            var result = _kscHrContext.OtherPaymentSetting.Where(x =>
            x.IsActive == true &&
            x.ValidityStartYearMonth <= startDate
            && (x.ValidityEndYearMonth >= endDate || x.ValidityEndYearMonth.HasValue == false)
            && x.OtherPaymentTypeId == otherPaymentType
            ).FirstOrDefault();
            return result;
        }

        //public bool IsExistActiveSetting(int? yearMonthStartReport, int? yearMonthEndReport, int accountCodeId)
        //{
        //    throw new NotImplementedException();
        //}
    }

}

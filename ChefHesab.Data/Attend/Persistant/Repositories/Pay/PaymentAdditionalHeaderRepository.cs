using Ksc.HR.Data.Persistant.Context;
using Ksc.HR.Domain.Entities.Pay;
using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Repositories;
using Ksc.HR.Domain.Repositories.Pay;
using Ksc.HR.Share.Model.emp;
using Ksc.HR.Share.Model.Employee;
using Ksc.HR.DTO.Pay.PaymentAdditionalHeader;
using Ksc.HR.DTO.WorkShift.IncludedDefinition;
using Ksc.HR.Share.Model.DeductionAdditional;
using KSC.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Domain.Entities.Salary;

namespace Ksc.HR.Data.Persistant.Repositories.Pay
{
    public class PaymentAdditionalHeaderRepository : EfRepository<PaymentAdditionalHeader, int>, IPaymentAdditionalHeaderRepository
    {

        private readonly KscHrContext _kscHrContext;
        public PaymentAdditionalHeaderRepository(KscHrContext KscHrContext) : base(KscHrContext)
        {
            _kscHrContext = KscHrContext;
        }
        public  IQueryable<PaymentAdditionalHeader> GetAllQueryable() => _kscHrContext.PaymentAdditionalHeaders.Include(a => a.PaymentAdditionalDetails);


        public long GetMartialSettingAmount(int employmentTypeId,int startdate, int endDate, int accountCode)
        {
            var query = _kscHrContext.InterdictMaritalSettingDetails
                .Include(x => x.InterdictMaritalSetting)
                .Where(x =>
                x.InterdictMaritalSetting.EmploymentTypeId== employmentTypeId &&
                x.AccountCodeId == accountCode &&
                x.MaritalStatusId == 1 &&
                x.InterdictMaritalSetting.StartDate == startdate && x.InterdictMaritalSetting.EndDate == endDate && x.IsActive==true
                ).FirstOrDefault();
            long val = query.Amount.Value;
            return val;
        }

        public int GetcountDaysWork(int monthTimeSheetYearMonth)
        {
            var countDaysWork = _kscHrContext.WorkCalendars.Count(a => a.YearMonthV1 == monthTimeSheetYearMonth);
            return countDaysWork;
        }

        public IQueryable<MonthTimeSheet> GetKindGartenRight(int date)
        {

            var query = _kscHrContext
                .MonthTimeSheets
                .AsNoTracking()
                .Where(x => x.YearMonth == date)
                .Include(x => x.MonthTimeSheetIncludeds)
                .Include(x => x.Employee)
                .ThenInclude(x => x.Families
                .Where(y =>
                y.DependenceTypeId == EnumDependentType.ChildBoye.Id ||
                y.DependenceTypeId == EnumDependentType.ChildGirl.Id)
                )
                .Where(x => x.Employee.Gender == EnumEmployeeGenderType.Woman.Id);
            return query;
        }

        public void RemovePaymentAdditionalHeaderAndIncluded(int yearmonth ,int accountCodeId)//(SearchPaymentAdditionalHeaderModel model)
        {
            if (_kscHrContext.PaymentAdditionalHeaders
                .Any(a => a.YearMonth == yearmonth
                 && a.AccountCodeId == accountCodeId))
            {
                var data = _kscHrContext.PaymentAdditionalHeaders.Where(a => a.YearMonth == yearmonth
                 && a.AccountCodeId == accountCodeId)
                    .Include(a => a.PaymentAdditionalDetails).First();
                _kscHrContext.PaymentAdditionalDetails.RemoveRange(data.PaymentAdditionalDetails);
                _kscHrContext.PaymentAdditionalHeaders.Remove(data);
            }
        }


        

        //public IQueryable<PaymentAdditionalHeader> GetPaymentAdditionals(int accountCodeId)
        //{
        //    var result = GetAllQueryable().Where(x => x.AccountCodeId == accountCodeId);
        //    return result;
        //}
    }
}

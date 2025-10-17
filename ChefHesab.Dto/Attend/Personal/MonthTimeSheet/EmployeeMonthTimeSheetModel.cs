using Ksc.HR.DTO.Personal.MonthTimeSheetRollCall;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.MonthTimeSheet
{
    public class EmployeeMonthTimeSheetModel
    {

        public int IsCreatedManualNum { get; set; }
        public int IsDeletedNum { get; set; }
        public string IsCreatedManualTitle { get; set; }
        public int? MonthTimeSheetID { get; set; }
        public int? EmployeeID { get; set; }
        public string EmployeeNumber { get; set; }
        //  public string CurrentUser { get; set; }
        /// <summary>
        /// کسر اضافه کار مازاد سقف ساعتی
        /// </summary>
        [DisplayName("کسر اضافه کار مازاد سقف ")]
        public int ExcessOverTimeHour { get; set; }
        /// <summary>
        /// کسر اضافه کار مازاد سقف دقیقه
        /// </summary>
        [DisplayName("کسر اضافه کار مازاد سقف ")]
        public int ExcessOverTimeMinute { get; set; }
        /// <summary>
        /// کسر اضافه کار تعدیل میانگین ساعت
        /// </summary>
        [DisplayName("کسر اضافه کار تعدیل میانگین")]

        public int AverageBalanceOverTimeHour { get; set; }
        /// <summary>
        /// کسر اضافه کار تعدیل میانگین دقیقه
        /// </summary>
        [DisplayName("کسر اضافه کار تعدیل میانگین")]
        public int AverageBalanceOverTimeMinute { get; set; }
        /// <summary>
        /// اضافه کار تایید نشده ساعت
        /// </summary>
        [DisplayName("اضافه کار تایید نشده")]
        public int SumInvalidOverTimInDailyTimeSheetHour { get; set; }
        /// <summary>
        /// اضافه کار تایید نشده دقیقه
        /// </summary>
        [DisplayName("اضافه کار تایید نشده")]
        public int SumInvalidOverTimInDailyTimeSheetMinute { get; set; }
        /// <summary>
        /// ایام کارکرد - قبلی
        /// </summary>
        public int? OldDailyWorkingdays { get; set; }
        /// <summary>
        /// ایام کارکرد - جاری
        /// </summary>
        public int? DailyWorkingdays { get; set; }
        /// <summary>
        /// ایام مشمول بیمه - قبلی
        /// </summary>
        public int? OldDailyInsurancePayment { get; set; }
        /// <summary>
        /// ایام مشمول بیمه - جاری
        /// </summary>
        public int? DailyInsurancePayment { get; set; }
        /// <summary>
        /// ایام مشمول مالیات - قبلی
        /// </summary>
        public int? OldDailyTaxPayment { get; set; }
        /// <summary>
        /// ایام مشمول مالیات - جاری
        /// </summary>
        public int? DailyTaxPayment { get; set; }
        /// <summary>
        /// ایام مشمول پاداش - قبلی
        /// </summary>
        public int? OldDailyReward { get; set; }
        /// <summary>
        /// ایام مشمول پاداش -جاری
        /// </summary>
        public int? DailyReward { get; set; }


        /// <summary>
        /// جمع ساعات عدم پرداخت حقوق - قدیم
        /// </summary>
        public string OldDailySalaryAggregateDeduction { get; set; }
        /// <summary>
        /// جمع ساعات عدم پرداخت حقوق - جدید
        /// </summary>
        public string DailySalaryAggregateDeduction { get; set; }
        /// <summary>
        /// مشمول کسر پاداش ساعتی - قدیم
        /// </summary>
        public string OldHourlyDeductionReward { get; set; }
        /// <summary>
        /// مشمول کسر پاداش ساعتی - جدید
        /// </summary>
        public string HourlyDeductionReward { get; set; }
        /// <summary>
        /// مشمول کسر پاداش روزانه- قدیم
        /// </summary>
        public int? OldDailyDeductionReward { get; set; }
        /// <summary>
        /// مشمول کسر پاداش روزانه - جدید
        /// </summary>
        public int? DailyDeductionReward { get; set; }
        /// <summary>
        ///  مشمول محاسبه در روزهای عیدی پذیر - قدیم
        /// </summary>
        public int? OldCalculationInEidDays { get; set; }

        /// <summary>
        ///  مشمول محاسبه در روزهای عیدی پذیر - جدید
        /// </summary>
        public int? DailyCalculationInEidDays { get; set; }
        /// <summary>
        /// بین راهی - قدیم
        /// </summary>
        public int? OldDailyBetweenAway { get; set; }

        /// <summary>
        /// بین راهی
        /// </summary>
        public int? DailyBetweenAway { get; set; }
        /// <summary>
        /// حضور در شیفت
        /// </summary>
        public List<MonthTimeSheetWorkTimeModel> MonthTimeSheetWorkTimesModel { get; set; }
        /// <summary>
        /// کارکرد ماهیانه
        /// </summary>
        public string MonthTimeSheetRollCallModels { get; set; }
        /// <summary>
        /// حضور در شیفت
        /// </summary>
        public string MonthTimeSheetWorkTimesModels { get; set; }

        /// <summary>
        /// مجموع اضافه کار
        /// </summary>
        public string SumDurationInMinutOverTime { get; set; }

        public double? ContDayForcurrentMonthMerit { get; set; }


        public string InsertUser { get; set; }
        public string YearMonth { get; set; }
        public bool CanEdit { get; set; }
        public string InsertPersianDate { get; set; }
        public string UpdatePersianDate { get; set; }
        public string UpdateUser { get; set; }
        public string Type { get; set; }
        public long Id { get; set; }
        public string DomainName { get; set; }
    }
    public class MonthTimeSheetWorkTimeModel
    {

        public string Code { get; set; }
        public string Title { get; set; }
        public int OldDayCount { get; set; } = 0;
        public int? DayCount { get; set; }
        public string Duration { get; set; }
        public int WorkTimeId { get; set; }
        public string AccountCode { get; set; }
    }
}

using Ksc.HR.Share.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.WorkShift.IncludedDefinition
{
    public class EnumIncludedDefinition : Enumeration
    {
        /// <summary>
        ///مشمول مرخصی استفاده شده
        /// </summary>
        public static readonly EnumIncludedDefinition UseedCurrentMonthVacation = new EnumIncludedDefinition(17, "مشمول مرخصی استفاده شده", null);

        /// <summary>
        ///مشمول مرخصی استحقاقی
        /// </summary>
        public static readonly EnumIncludedDefinition CurrentMonthMerit = new EnumIncludedDefinition(18, "مشمول مرخصی استحقاقی", null);



        /// <summary>
        /// اضافه کاری مشمول سقف
        /// </summary>
        public static readonly EnumIncludedDefinition MaximunOverTime = new EnumIncludedDefinition(1, "اضافه کاری مشمول سقف", null);

        /// <summary>
        /// اضافه کاری مشمول میانگین
        /// </summary>
        public static readonly EnumIncludedDefinition AverageOverTime = new EnumIncludedDefinition(2, "اضافه کاری مشمول میانگین", null);

        /// <summary>
        /// مزایای حق شیفت
        /// </summary>
        public static readonly EnumIncludedDefinition ShiftAdvantages = new EnumIncludedDefinition(3, "مزایای حق شیفت", null);

        /// <summary>
        /// مشمول کسر پاداش ساعتی
        /// </summary>
        public static readonly EnumIncludedDefinition HourlyDeductionReward = new EnumIncludedDefinition(4, "مشمول کسر پاداش ساعتی", null);

        /// <summary>
        /// مشمول کسر پاداش روزانه
        /// </summary>
        public static readonly EnumIncludedDefinition DailyDeductionReward = new EnumIncludedDefinition(5, "مشمول کسر پاداش روزانه", null);

        /// <summary>
        /// مشمول پرداخت بیمه
        /// </summary>
        public static readonly EnumIncludedDefinition InsurancePayment = new EnumIncludedDefinition(6, "مشمول پرداخت بیمه", null);
        /// <summary>
        /// مشمول پرداخت مالیات
        /// </summary>
        public static readonly EnumIncludedDefinition TaxPayment = new EnumIncludedDefinition(7, "مشمول پرداخت مالیات", null);
        /// <summary>
        /// مشمول کسر حقوق منفرد
        /// </summary>
        public static readonly EnumIncludedDefinition SalarySingleDeduction = new EnumIncludedDefinition(8, "مشمول کسر حقوق منفرد", null);
        /// <summary>
        /// مشمول کسر حقوق تجمعی
        /// </summary>
        public static readonly EnumIncludedDefinition SalaryAggregateDeduction = new EnumIncludedDefinition(9, "مشمول کسر حقوق تجمعی", null);
        /// <summary>
        /// جزء ایام کارکرد
        /// </summary>
        public static readonly EnumIncludedDefinition Workingdays = new EnumIncludedDefinition(10, "جزء ایام کارکرد", null);

        /// <summary>
        /// مشمول حق بین راهی
        /// </summary>
        public static readonly EnumIncludedDefinition BetweenAway = new EnumIncludedDefinition(11, "مشمول حق بین راهی", null); 
        /// <summary>
        /// مشمول مرخصی
        /// </summary>
        public static readonly EnumIncludedDefinition Vacation = new EnumIncludedDefinition(12, "مشمول مرخصی", null);
        /// <summary>
        /// مشمول محاسبه در روزهای عیدی پذیر
        /// </summary>
        public static readonly EnumIncludedDefinition CalculationInEidDays = new EnumIncludedDefinition(13, "مشمول محاسبه در روزهای عیدی پذیر", null);


        /// <summary>
        /// مشمول اضافه کار قهری
        /// </summary>
        public static readonly EnumIncludedDefinition ForcedOverTime = new EnumIncludedDefinition(16, "مشمول اضافه کار قهری", null);
        /// <summary>
        /// مشمول اضافه کار پنجشنبه تعطیل رسمی
        /// </summary>
        public static readonly EnumIncludedDefinition ThursdayOfficalHoliadyIncludedOverTime = new EnumIncludedDefinition(17, "مشمول اضافه کار پنجشنبه تعطیل رسمی", null);
        /// <summary>
        /// مشمول محاسبه پاداش تولید
        /// </summary>
        public static readonly EnumIncludedDefinition CaculateReward = new EnumIncludedDefinition(27, "مشمول محاسبه پاداش تولید", null);

        public EnumIncludedDefinition(int id, string name, string group)
    : base(id, name, group)
        {
        }
    }
}

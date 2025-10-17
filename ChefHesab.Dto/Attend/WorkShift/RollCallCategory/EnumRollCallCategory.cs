using Ksc.HR.Share.General;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.WorkShift.RollCallCategory
{

    /// <summary>
    /// دسته بندی کارکرد
    /// </summary>
    public class EnumRollCallCategory : Enumeration
    {
        /// <summary>
        /// کارکرد 
        /// </summary>
        public static readonly EnumRollCallCategory Karkard = new EnumRollCallCategory(1, "کارکرد", null);

        /// <summary>
        /// استعلاجی 
        /// </summary>
        public static readonly EnumRollCallCategory Estelagi = new EnumRollCallCategory(2, "استعلاجی", null);
        /// <summary>
        /// عدم حضور طولانی
        /// </summary>
        public static readonly EnumRollCallCategory LongTermAbsence = new EnumRollCallCategory(3, "عدم حضور طولانی", null);
        /// <summary>
        /// کارکرد
        /// </summary>
        public static readonly EnumRollCallCategory VacationHours = new EnumRollCallCategory(5, "مرخصی استحقاقی ساعتی", null);
        /// <summary>
        /// مرخصی
        /// </summary>
        public static readonly EnumRollCallCategory VacationDaily = new EnumRollCallCategory(6, "مرخصی استحقاقی روزانه", null);
        /// <summary>
        /// اضافه کاری
        /// </summary>
        public static readonly EnumRollCallCategory OverTime = new EnumRollCallCategory(8, "اضافه کاری", null);
        /// <summary>
        /// اضافه کار تعطیلی
        /// </summary>
        public static readonly EnumRollCallCategory OverTimeInHoliday = new EnumRollCallCategory(9, "اضافه کار تعطیلی", null);
        /// <summary>
        /// آموزش
        /// </summary>
        public static readonly EnumRollCallCategory Training = new EnumRollCallCategory(10, "آموزش", null);
        /// <summary>
        /// کارکرد در تعطیلات رسمی 
        /// </summary>
        public static readonly EnumRollCallCategory KarkardInOfficialHoliday = new EnumRollCallCategory(11, "کارکرد در تعطیلات رسمی", null);
        /// <summary>
        /// کارکرد در تعطیلات غیر رسمی 
        /// </summary>
        public static readonly EnumRollCallCategory KarkardInUnOfficialHoliday = new EnumRollCallCategory(12, "کارکرد در تعطیلات غیر رسمی", null);
        /// <summary>
        /// اضافه کار پیشفرض 
        /// </summary>
        public static readonly EnumRollCallCategory DefaultOverTime = new EnumRollCallCategory(13, "اضافه کار پیشفرض", null);
        /// <summary>
        /// فرجه شير دهي 
        /// </summary>
        public static readonly EnumRollCallCategory BreastfeddingTolerance = new EnumRollCallCategory(14, "فرجه شير دهي", null);
        /// <summary>
        /// غيبت ساعتي 
        /// </summary>
        public static readonly EnumRollCallCategory HourlyAbsence = new EnumRollCallCategory(16, "غيبت ساعتي", null);

        /// <summary>
        /// ماموریت
        /// </summary>
        public static readonly EnumRollCallCategory Mission = new EnumRollCallCategory(17, "ماموریت", null);
        /// <summary>
        /// اضافه کار تعطیلی استانی
        /// </summary>
        public static readonly EnumRollCallCategory OverTimeInUnOfficialHoliday = new EnumRollCallCategory(22, "اضافه کار تعطیلی استانی", null);
        public EnumRollCallCategory(int id, string name, string group)
    : base(id, name, group)
        {
        }

    }
}
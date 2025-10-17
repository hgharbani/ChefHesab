using Ksc.HR.Share.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ksc.HR.DTO.WorkFlow.Classes.Enumerations
{

    /// <summary>
    /// نوع روز در تقویم
    /// </summary>
    public class EnumWorkDayType : Enumeration
    {

        /// <summary>
        /// روز عادی 
        /// </summary>
        public static readonly EnumWorkDayType NormalDay = new EnumWorkDayType(1, "روز عادی", null);
        /// <summary>
        /// تعطیل رسمی
        /// </summary>
        public static readonly EnumWorkDayType OfficialHoliday = new EnumWorkDayType(3, "تعطیل رسمی", null);
        /// <summary>
        /// تعطیل استانی
        /// </summary>
        public static readonly EnumWorkDayType UnOfficialHoliday = new EnumWorkDayType(5, "تعطیل استانی", null);
        /// <summary>
        /// تعطیل روز جهانی کارگر
        /// </summary>
        public static readonly EnumWorkDayType WorkerDayHoliday = new EnumWorkDayType(6, "تعطیل روز جهانی کارگر", null);
        /// <summary>
        /// پنجشنبه
        /// </summary>
        public static readonly EnumWorkDayType Thursday = new EnumWorkDayType(7, "پنجشنبه", null);
        /// <summary>
        /// جمعه
        /// </summary>
        public static readonly EnumWorkDayType Friday = new EnumWorkDayType(8, "جمعه", null);
        /// <summary>
        /// تعطیل رسمی در پنجشنبه
        /// </summary>
        public static readonly EnumWorkDayType OfficialHolidayInThursday = new EnumWorkDayType(9, "تعطیل رسمی در پنجشنبه", null);
        /// <summary>
        /// تعطیل رسمی در جمعه
        /// </summary>
        public static readonly EnumWorkDayType OfficialHolidayInFriday = new EnumWorkDayType(10, "تعطیل رسمی در جمعه", null);

        public EnumWorkDayType(int id, string name, string group)
    : base(id, name, group)
        {
        }

    }
}
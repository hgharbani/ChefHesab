using Ksc.HR.Share.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.WorkShift.SacrificeOptionSetting
{
    public class EnumSacrificeOptionSetting : Enumeration
    {
        /// <summary>
        /// اضافه کارجانبازی ( تایید کارکرد روزانه) 
        /// </summary>
        public static readonly EnumSacrificeOptionSetting SacrificeOverTime = new EnumSacrificeOptionSetting(1, "اضافه کارجانبازی ( تایید کارکرد روزانه)", null);
        /// <summary>
        /// مرخصی (در برنامه بروزرسانی مرخصی استفاده می شود) 
        /// </summary>
        public static readonly EnumSacrificeOptionSetting SacrificeVacation = new EnumSacrificeOptionSetting(2, "مرخصی (در برنامه بروزرسانی مرخصی استفاده می شود)", null);
        /// <summary>
        /// سنوات افزوده ( کاربردی در حضور و غیاب ندارد) 
        /// </summary>
        public static readonly EnumSacrificeOptionSetting SacrificeSumYears = new EnumSacrificeOptionSetting(3, "سنوات افزوده ( کاربردی در حضور و غیاب ندارد)", null);
        /// <summary>
        /// تاخیر در ورود جانبازی 
        /// </summary>
        public static readonly EnumSacrificeOptionSetting SacrificeEntrance = new EnumSacrificeOptionSetting(4, "تاخیر در ورود جانبازی", null);
        /// <summary>
        /// تعجیل در خروج جانبازی 
        /// </summary>
        public static readonly EnumSacrificeOptionSetting SacrificeExit = new EnumSacrificeOptionSetting(5, "تعجیل در خروج جانبازی", null);

        public EnumSacrificeOptionSetting(int id, string name, string group) : base(id, name, group)
        {
        }
    }

}

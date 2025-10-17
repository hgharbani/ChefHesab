using Ksc.HR.Share.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ksc.HR.DTO.WorkFlow.Classes.Enumerations
{

    /// <summary>
    /// نوع تقویم
    /// </summary>
    public class EnumCalendarType : Enumeration
    {

        /// <summary>
        /// شمسی 
        /// </summary>
        public static readonly EnumCalendarType Shamsi = new EnumCalendarType(0, "شمسی", null);
        /// <summary>
        /// میلادی
        /// </summary>
        public static readonly EnumCalendarType Miladi = new EnumCalendarType(1, "میلادی", null);
        /// <summary>
        /// قمری
        /// </summary>
        public static readonly EnumCalendarType Ghamari = new EnumCalendarType(2, "قمری", null);

        public EnumCalendarType(int id, string name, string group)
    : base(id, name, group)
        {
        }

    }
}
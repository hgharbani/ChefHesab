using Ksc.HR.Share.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ksc.HR.DTO.WorkFlow.Classes.Enumerations
{

    /// <summary>
    /// نوع کدهای سازگار
    /// </summary>
    public class EnumCompatibleRollCallType : Enumeration
    {

        /// <summary>
        /// کدهای سازگار با هم 
        /// </summary>
        public static readonly EnumCompatibleRollCallType Compatible = new EnumCompatibleRollCallType(1, "کدهای سازگار با هم", null);
        /// <summary>
        /// کدهای قابل تبدیل به یکدیگر
        /// </summary>
        public static readonly EnumCompatibleRollCallType Interchangeable = new EnumCompatibleRollCallType(2, "کدهای قابل تبدیل به یکدیگر", null); 
        /// <summary>
        /// کدهای که منجر به ایجاد کد دیگر در تایم شیت میشوند
        /// </summary>
        public static readonly EnumCompatibleRollCallType AddNewRowInTimeSheet = new EnumCompatibleRollCallType(3, "کدهای که منجر به ایجاد کد دیگر در تایم شیت میشوند", null);

        public EnumCompatibleRollCallType(int id, string name, string group)
    : base(id, name, group)
        {
        }

    }
}
using Ksc.HR.Share.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.WorkShift.MonthTimeSheetLog
{
    public class EnumMonthTimeSheetStepType : Enumeration
    {
        /// <summary>
        /// اضافه کاری مشمول سقف
        /// </summary>
        public static readonly EnumMonthTimeSheetStepType MaximunOverTime = new EnumMonthTimeSheetStepType(1, "اضافه کاری مشمول سقف", null);
        public EnumMonthTimeSheetStepType(int id, string name,string group) : base(id, name,group)
        {
        }
    }
   
}

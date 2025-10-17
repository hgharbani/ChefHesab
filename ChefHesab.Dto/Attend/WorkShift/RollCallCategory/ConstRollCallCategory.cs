using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ksc.HR.DTO.WorkShift.RollCallCategory
{

    /// <summary>
    ///  دسته بندی کارکرد
    /// </summary>
    public static class ConstRollCallCategory
    {
        /// <summary>
        /// اضافه کار در تایید کارکرد(سمت چپ )و پورتال 
        /// </summary>
        public static List<int> OverTime
        {
            get
            {
                return new List<int>() {
                EnumRollCallCategory.OverTime.Id , EnumRollCallCategory.DefaultOverTime.Id ,
                EnumRollCallCategory.OverTimeInHoliday.Id,
                EnumRollCallCategory.OverTimeInUnOfficialHoliday.Id
                };
            }
        }


    }
}
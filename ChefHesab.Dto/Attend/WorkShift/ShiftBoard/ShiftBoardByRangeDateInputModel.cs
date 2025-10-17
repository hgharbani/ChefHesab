using System;
using System.Collections.Generic;
using System.Text;

namespace Ksc.HR.DTO.WorkShift.ShiftBoard
{
    public class ShiftBoardByRangeDateInputModel
    {
        /// <summary>
        /// از تاریخ
        /// </summary>
        public DateTime DateFrom { get; set; }
        /// <summary>
        ///  تا تاریخ
        /// </summary>
        public DateTime DateTo { get; set; }
    }
}

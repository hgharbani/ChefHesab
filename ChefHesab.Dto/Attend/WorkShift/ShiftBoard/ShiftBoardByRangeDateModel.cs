using System;
using System.Collections.Generic;
using System.Text;

namespace Ksc.HR.DTO.WorkShift.ShiftBoard
{
    public class ShiftBoardByRangeDateModel
    {
        public int ShamsiYear { get; set; }
        public int ShamsiDate { get; set; }
        public DateTime MiladiDate { get; set; }
        public string DayNameShamsi { get; set; }
        public string WorkTime { get; set; } 
        public string WorkGroupTitle { get; set; }
        public int ShiftCode { get; set; } 
        public string ShiftTitle { get; set; }
        public string ShiftStartTime { get; set; }
        public string ShiftEndtTime { get; set; }
        public int ShiftConceptId { get; set; } 
        public bool IsRest { get; set; } 
        public DateTime ShiftStartDate { get; set; }
        public DateTime? ShiftEndDate { get; set; } 
    }
}

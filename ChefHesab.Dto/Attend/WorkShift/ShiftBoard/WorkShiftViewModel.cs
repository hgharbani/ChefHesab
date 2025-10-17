using System;
using System.Collections.Generic;

namespace Ksc.HR.DTO.WorkShift.ShiftBoard
{
    public class WorkShiftViewModel
    {
        public WorkShiftViewModel()
        {
            DaysShifts =new List<WorkShiftItemViewModel>();

        }
        public string ShiftName { get; set; }
        public string YYYYMM { get; set; }
        public DateTime? TimeSheetMonth { get; set; }
        public List<WorkShiftItemViewModel> DaysShifts { get; set; }
    }

    public class WorkShiftItemViewModel
    {
        public string ShiftTime { get; set; }
        public int DayId { get; set; }
        public int ColorId { get; set; }
    }

    public class searchMonthShift
    {

        public string date { get; set; }
        public string shift { get; set; }
    }
}
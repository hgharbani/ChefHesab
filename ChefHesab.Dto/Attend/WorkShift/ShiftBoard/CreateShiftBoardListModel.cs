using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.WorkShift.ShiftBoard
{
    public class CreateShiftBoardListModel
    {
        public string ShiftDate { get; set; }
        public string ShiftDay { get; set; }
        public int ShiftConceptDetailId { get; set; }
        public string ShiftStartTime { get; set; }
        public string ShiftEndTime { get; set; }
        public int ShiftBoardId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.WorkShift.ShiftBoard
{
    public class CreateWorkShiftInputModel
    {
        public int WorkGroupId { get;set;}
        public string Date { get;set;}
        public int RepetitionPeriod { get;set;}
    }
}

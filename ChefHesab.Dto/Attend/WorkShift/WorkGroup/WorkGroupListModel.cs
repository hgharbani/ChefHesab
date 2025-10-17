using Ksc.HR.DTO.WorkShift.WorkTimeCategory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.DTO.WorkShift.Province;

namespace Ksc.HR.DTO.WorkShift.WorkGroup
{
    public class WorkGroupListModel
    {
        //public WorkGroup()
        //{
        //    ShiftBoard = new HashSet<ShiftBoard>();
        //}

        public int Id { get; set; }
        public string Code { get; set; }
        public string WorkTimeTitle { get; set; }
        public int? RepetitionPeriod { get; set; }
        public string WorkGroupTitle { get; set; }



    }
}

using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.WorkShift.ShiftBoard
{
    public class ShiftBoardGridSearchModel: FilterRequest
    {
        public int WorkGroupId { get;set;}
        public string Date { get;set;}
    }
}

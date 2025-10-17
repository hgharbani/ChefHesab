using KSC.Common.Filters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.WorkShift.RollCallDefinication
{
    public class RollCallDefinicationFilterRequest : FilterRequest
    {
        public bool IsValidForTemporaryStartDate { get; set; }
        public bool IsValidForTemporaryEndDate { get; set; }
    }
}

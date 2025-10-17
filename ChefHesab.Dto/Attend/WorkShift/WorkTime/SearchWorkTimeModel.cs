using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.WorkShift.WorkTime
{
    public class SearchWorkTimeModel
    {
        public int Id { get; set; } // Id (Primary key)
        public string Title { get; set; } // Title (length: 200)
        public string Code { get; set; } // Code (length: 50)
      

    }
}

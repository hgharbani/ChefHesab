using Ksc.HR.DTO.Other;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ksc.HR.DTO.WorkShift

{
    public class SearchPaymentStatusModel : SelectListItem
    {
       
        public int Id { get; set; } // Id (Primary key)

      public string Title { get; set; } // Title (length: 50)

        public bool IsActive { get; set; } // IsActive

    }
}




using Ksc.HR.DTO.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ksc.HR.DTO.WorkShift.Country
{
    public class SearchCountryModel:SelectListItem
    {
        public int Id { get; set; } // Id (Primary key)
        public string Title { get; set; } // Title (length: 200)
        public string Code { get; set; } // Code (length: 50)



    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.WorkShift.Country
{
    public class AddOrEditCountryModel
    {
        public int Id { get; set; } 
        [DisplayName("عنوان")]
        public string Title { get; set; }
        [DisplayName("کد")]
        public string Code { get; set; } 
        //public DateTime? InsertDate { get; set; } 
        public string CurrentUserName { get; set; }
       public string DomainName { get; set; } // DomainName (length: 50)

        public int? OrderNo { get; set; }
    }
}

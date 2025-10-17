
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ksc.HR.DTO.WorkShift.Country
{
    public class EditCountryModel
    {
        //public EditCountryModel()
        //{
        //    AvailbleProvince = new List<SearchProvinceModel>();
        //}
        public int Id { get; set; } // Id (Primary key)

        [DisplayName("عنوان")]
        public string Title { get; set; } // Title (length: 200)

        [DisplayName("کد")]
        public string Code { get; set; } // Code (length: 50)
        //public int ProvinceId { get; set; } // WorkTimeCategoryId

        //[DisplayName("دوره تکرار")] 
        //public int RepetitionPeriod { get; set; } // RepetitionPeriod
        public DateTime? InsertDate { get; set; } // InsertDate
        public DateTime? UpdateDate { get; set; } // UpdateDate
        public string CurrentUserName { get; set; }
        public string DomainName { get; set; } // DomainName (length: 50)
        public bool IsActive { get; set; } // IsActive
        //public List<SearchProvinceModel> AvailbleProvince { get; set; }

    }
}

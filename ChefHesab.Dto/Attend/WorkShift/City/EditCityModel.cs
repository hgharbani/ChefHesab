using Ksc.HR.DTO.WorkShift.WorkTimeCategory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.DTO.WorkShift.Province;
using Ksc.HR.DTO.WorkShift.Country;

namespace Ksc.HR.DTO.WorkShift.City
{
    public class EditCityModel
    {
        public EditCityModel()
        {
            AvailbleProvince = new List<SearchProvinceModel>();
        }
        public int Id { get; set; } // Id (Primary key)
        
        [DisplayName("عنوان")] 
        public string Title { get; set; } // Title (length: 200)

        [DisplayName("کد")] 
        public string Code { get; set; } // Code (length: 50)

        public int CountryId { get; set; } // WorkTimeCategoryId

        public int ProvinceId { get; set; } // WorkTimeCategoryId
        public List<SearchProvinceModel> Provinces { get; set; }
        public List<SearchCountryModel> Countries { get; set; }

        [DisplayName("دوره تکرار")] 
        public int RepetitionPeriod { get; set; } // RepetitionPeriod
        public DateTime? InsertDate { get; set; } // InsertDate
        public DateTime? UpdateDate { get; set; } // UpdateDate
        public string CurrentUserName { get; set; }
        public string DomainName { get; set; } // DomainName (length: 50)
        public bool IsActive { get; set; } // IsActive

        public List<SearchProvinceModel> AvailbleProvince { get; set; }

    }
}

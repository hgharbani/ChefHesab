using Ksc.HR.DTO.WorkShift.Country;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.WorkShift.Province
{
    public class EditProvinceModel
    {
        //public AddProvinceModel()
        //{
        //    AvailbleProvinceCountries = new List<SearchCountryModel>();
        //}
        public int Id { get; set; } // Id (Primary key)

        [DisplayName("عنوان")]
        public string Title { get; set; } // Title (length: 200)

        [DisplayName("کد")]
        public string Code { get; set; } // Code (length: 50)

        public int CountryId { get; set; } // CountryId

        [DisplayName("دوره تکرار")]
        public int RepetitionPeriod { get; set; } // RepetitionPeriod

        public DateTime? InsertDate { get; set; } // InsertDate


        public DateTime? UpdateDate { get; set; } // UpdateDate

        public string CurrentUserName { get; set; }

        public string DomainName { get; set; } // DomainName (length: 50)

        public bool IsActive { get; set; } // IsActive
        public List<SearchCountryModel> Counties { get; set; }


        // public List<SearchCountryModel> AvailbleProvinceCategori { get; set; }
    }
}

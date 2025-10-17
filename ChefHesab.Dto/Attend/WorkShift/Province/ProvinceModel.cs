using System;
using System.ComponentModel;

namespace Ksc.HR.DTO.WorkShift.Province
{
    public class ProvinceModel
    {
        //public ProvinceModel()
        //{
        //    AvailbleProvinceCategori = new List<SearchProvinceCategoryModel>();
        //}
        public int Id { get; set; }

        [DisplayName("عنوان")] 
        public string Title { get; set; } // Title (length: 200)


        [DisplayName("کشور")]
        public string Country { get; set; } // Title (length: 200)

        [DisplayName("کد")]
        public string Code { get; set; } // Code (length: 50)
        public int? CountryId { get; set; }


        //[DisplayName("عنوان")] 
        //public int ProvinceCategoryId { get; set; } // ProvinceCategoryId

        //[DisplayName("عنوان")] 
        //public int RepetitionPeriod { get; set; } // RepetitionPeriod

        [DisplayName("تاریخ درج")] 
        public DateTime? InsertDate { get; set; } // InsertDate

        [DisplayName("کاربر")] 
        public string InsertUser { get; set; } // InsertUser (length: 50)

        [DisplayName("عنوان")] 
        public DateTime? UpdateDate { get; set; } // UpdateDate

        [DisplayName("عنوان")] 
        public string UpdateUser { get; set; } // UpdateUser (length: 50)

        //[DisplayName("عنوان")] 
        //public string DomainName { get; set; } // DomainName (length: 50)

        //public bool IsActive { get; set; } // IsActive

        //[DisplayName("دسته بندی زمان کاری")] 
        //public string ProvinceCategoryTitle { get; set; }

        //[DisplayName("کد دسته بندی زمان کاری")] 
        //public string ProvinceCategoryCode { get; set; }
       // public List<SearchProvinceCategoryModel> AvailbleProvinceCategori { get; set; }
    }
}

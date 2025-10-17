using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ksc.HR.DTO.WorkShift.WorkCompanySetting
{
    public class SearchWorkCompanySettingModel
    {
        public int Id { get; set; }
        [DisplayName("زمان کاری")] 
        public int? WorkTimeId { get; set; }
        [DisplayName("زمان کاری")]
        public string WorkTimeTitle { get; set; }
        [DisplayName("ماهیت")]
        public string ShiftConceptTitle { get; set; }

        [DisplayName("شهر")]
        public string CityName { get; set; }

        [DisplayName("شرکت")]
        public string CompanyName { get; set; }
        public string WorkCompanySettingTitle { get; set; }

    }
}

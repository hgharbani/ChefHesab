
using Ksc.HR.DTO.WorkShift.City;
using Ksc.HR.DTO.WorkShift.ShiftConcept;
using Ksc.HR.DTO.WorkShift.ShiftHoliday;
using Ksc.HR.DTO.WorkShift.TimeShiftSetting;
using Ksc.HR.DTO.WorkShift.WorkTime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.WorkShift.WorkCompanySetting
{
    public class AddWorkCompanySettingModel
    {
        public AddWorkCompanySettingModel()
        {
            ShiftHolidayList = new List<AddShiftHolidayModel>();
            TimeShiftSettingList = new HashSet<AddTimeShiftSettingModel>();
            //AvilableListWorkTime = new List<SearchWorkTimeModel>();
            //AvilableShiftConceptModel = new List<SearchShiftConceptModel>();
            AvilableCityModel = new List<SearchCityModel>();
        }

        public int Id { get; set; }

        //[DisplayName("زمان کاری")]
        //public int? WorkTimeId { get; set; }

        //public string WorkTimeTitle { get; set; }

        //[DisplayName("ماهیت شیفت")]
        //public int? ShiftConceptId { get; set; }
        public int WorkTimeShiftConceptId { get; set; }

        [DisplayName("شهر")]
        public int? WorkCityId { get; set; }
        public int? CompnayId { get; set; }
        public string CompanyName { get; set; }
        public DateTime? InsertDate { get; set; }
        public string InsertDateString { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string CurrentUserName { get; set; }
        public string? DomainName { get; set; }
        public bool IsActive { get; set; }
        //public byte[] RowVersion { get; set; } = null!;

        public List<AddShiftHolidayModel> ShiftHolidayList { get; set; }
        public ICollection<AddTimeShiftSettingModel> TimeShiftSettingList { get; set; }
        //public List<SearchWorkTimeModel> AvilableListWorkTime { get; set; }
        //public List<SearchShiftConceptModel> AvilableShiftConceptModel { get; set; }
        //public List<SearchWorkTimeModel> AvilableListWorkTime { get; set; }

        public List<SearchCityModel> AvilableCityModel { get; set; }
    }
}

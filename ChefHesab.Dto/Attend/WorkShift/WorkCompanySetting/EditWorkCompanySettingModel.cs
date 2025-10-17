using Ksc.HR.DTO.WorkShift.WorkTimeCategory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.DTO.WorkShift.Province;
using Ksc.HR.DTO.WorkShift.ShiftHoliday;
using Ksc.HR.DTO.WorkShift.TimeShiftSetting;

namespace Ksc.HR.DTO.WorkShift.WorkCompanySetting
{
    public class EditWorkCompanySettingModel
    {
        public EditWorkCompanySettingModel()
        {
            ShiftConceptList = new List<AddShiftHolidayModel>();
            TimeShiftSettingList = new HashSet<AddTimeShiftSettingModel>();
        }

        public int Id { get; set; }
        public int? WorkTimeId { get; set; }
        public string WorkTimeTitle { get; set; }
        public int? ShiftConceptId { get; set; }
        public int? WorkCityId { get; set; }
        public int? CompnayId { get; set; }
        public string CompanyName { get; set; }
        public DateTime? InsertDate { get; set; }
        public string InsertDateString { get; set; }
                public DateTime? UpdateDate { get; set; }
        public string CurrentUserName { get; set; }
        public string? DomainName { get; set; }
        public bool IsActive { get; set; }
        public byte[] RowVersion { get; set; } = null!;

        public List<AddShiftHolidayModel> ShiftConceptList { get; set; }
        public virtual ICollection<AddTimeShiftSettingModel> TimeShiftSettingList { get; set; }

    }
}

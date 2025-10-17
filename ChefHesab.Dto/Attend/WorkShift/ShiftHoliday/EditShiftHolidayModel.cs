
using Ksc.HR.DTO.Other;
using Ksc.HR.Share.Extention;
using Ksc.HR.Share.General;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ksc.HR.DTO.WorkShift.ShiftHoliday
{
    public class EditShiftHolidayModel
    {
        //public EditShiftHolidayModel()
        //{
        //    AvailbleProvince = new List<SearchProvinceModel>();
        //}
        public int Id { get; set; }
        public int WorkCompanySettingId { get; set; }

        [DisplayName("شماره روز")]
        public DayNumberType DayNumber { get; set; }
        [DisplayName("شماره روز")]
        public string DayNumberString
        {
            get
            {
                return DayNumber.GetDisplayName();
            }
        }

        [DisplayName("تاریخ درج")]
        public DateTime? InsertDate { get; set; }
        [DisplayName("تاریخ ویرایش")]
        public DateTime? UpdateDate { get; set; }
        public string CurrentUserName { get; set; }
        [DisplayName("دامنه")]
        public string? DomainName { get; set; }
        [DisplayName("وضعیت")]
        public bool IsActive { get; set; }
        public byte[] RowVersion { get; set; }
        public List<SelectListItem> AvailbleDayNumberTypes { get; set; }

        // public virtual WorkCompanySetting WorkCompanySetting { get; set; }

    }
}


using KSC.Common.Filters.Models;
using Ksc.HR.DTO.Utility.Extentions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.DTO.Entities.Enumrations;
using Ksc.HR.DTO.Other;
using Ksc.HR.Share.General;
using Ksc.HR.Share.Extention;

namespace Ksc.HR.DTO.WorkShift.ShiftHoliday
{
    public class ShiftHolidayModel:FilterRequest
    {
        //public ShiftHolidayModel()
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
        [DisplayName("کاربر ثبت کننده")]
        public string? InsertUser { get; set; }
        [DisplayName("تاریخ ویرایش")]
        public DateTime? UpdateDate { get; set; }
        [DisplayName("کاربر ویرایش کننده")]
        public string? UpdateUser { get; set; }
        [DisplayName("دامنه")]
        public string? DomainName { get; set; }
        [DisplayName("وضعیت")]
        public bool IsActive { get; set; }

        public byte[] RowVersion { get; set; }

        public List<SelectListItem> AvailbleDayNumberTypes { get; set; }

        // public virtual WorkCompanySetting WorkCompanySetting { get; set; }
    }
}


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ksc.HR.DTO.WorkShift.WorkCompanySetting
{
    public class WorkCompanySettingModel
    {
     
        public int Id { get; set; }
        public int? WorkTimeId { get; set; }
        public string WorkTimeTitle { get; set; }
        public int? ShiftConceptId { get; set; }
        public int? WorkCityId { get; set; }
        public int? CompnayId { get; set; }
        public string CompanyName { get; set; }
        public DateTime? InsertDate { get; set; }
        public string InsertDateString { get; set; }
        public string? InsertUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateUser { get; set; }
        public string? DomainName { get; set; }
        public bool IsActive { get; set; }
        public byte[] RowVersion { get; set; } = null!;

        //public virtual ShiftConcept? ShiftConcept { get; set; }
        //public virtual City? WorkCity { get; set; }
        //public virtual WorkTime? WorkTime { get; set; }
        //public virtual ICollection<ShiftHoliday> ShiftHoliday { get; set; }
        //public virtual ICollection<TimeShiftSetting> TimeShiftSetting { get; set; }
    }
}

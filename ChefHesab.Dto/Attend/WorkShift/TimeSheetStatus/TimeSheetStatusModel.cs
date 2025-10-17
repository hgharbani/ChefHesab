
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ksc.HR.DTO.WorkShift.TimeSheetStatus
{
    public class TimeSheetStatusModel
    {
        //public TimeSheetStatus()
        //{
        //    WorkCalendar = new HashSet<WorkCalendar>();
        //}

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Code { get; set; } = null!;
        public DateTime? InsertDate { get; set; }
        public string? InsertUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateUser { get; set; }
        public string? DomainName { get; set; }
        public bool IsActive { get; set; }
        public byte[] RowVersion { get; set; } = null!;

        //public virtual ICollection<WorkCalendar> WorkCalendar { get; set; }
    }
}

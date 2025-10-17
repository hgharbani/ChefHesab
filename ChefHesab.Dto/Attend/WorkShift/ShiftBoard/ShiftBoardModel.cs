
using Ksc.HR.DTO.WorkShift.ShiftConcept;
using Ksc.HR.DTO.WorkShift.ShiftConceptDetail;
using Ksc.HR.DTO.WorkShift.WorkCalendar;
using Ksc.HR.DTO.WorkShift.WorkGroup;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.WorkShift.ShiftBoard
{
    public class ShiftBoardModel
    {
        //public ShiftBoardModel()
        //{
        //    AvailableShiftConcept = new List<SearchShiftConceptModel>();
        //}

        public ShiftBoardModel()
        {
            AvailbleShiftConceptDetail = new List<SearchShiftConceptDetailModel>();
            AvailbleWorkGroup = new List<SearchWorkGroupModel>();
            AvailbleWorkCalendar = new List<SearchWorkCalendarModel>();

        }
        public int Id { get; set; }
        public int WorkGroupId { get; set; }
        public int ShiftBoardId { get; set; }
        public int WorkCalendarId { get; set; }
        
        public int ShiftConceptDetailId { get; set; }
        public string YyyyshamsiTitle { get; set; }
        public string YearMonthV1Title { get; set; }
        public string StartTime { get; set; }
        public string EndTimeTime { get; set; }
        

        [DisplayName("گروه کاری")]
        public string WorkGroupTitle { get; set; }

        [DisplayName("ماهیت شیفت")]
        public string ShiftConceptDetailTitle { get; set; }


        /// <summary>
        /// شروع دوره است ؟
        /// </summary>
        public bool IsPeriodStartDate { get; set; }
        public DateTime? InsertDate { get; set; }
        public string InsertUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public string DomainName { get; set; }
        public bool IsActive { get; set; }
        public byte[] RowVersion { get; set; }
        public List<SearchShiftConceptDetailModel> AvailbleShiftConceptDetail { get; set; }
        public List<SearchWorkGroupModel> AvailbleWorkGroup { get; set; }
        public List<SearchWorkCalendarModel> AvailbleWorkCalendar { get; set; }
        public string WorkTimeTitle { get;set; }
        public int? RepetitionPeriod { get; set; }
        //public virtual ShiftBoard ShiftBoard { get; set; }
        //public virtual WorkCalendar WorkCalendar { get; set; }
        //public virtual WorkGroup WorkGroup { get; set; }

    }
}

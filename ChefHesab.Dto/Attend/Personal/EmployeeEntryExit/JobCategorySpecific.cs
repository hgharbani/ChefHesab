using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.EmployeeEntryExit
{
    public class JobCategorySpecificModel
    {
        public JobCategorySpecificModel()
        {
            List<JobCategorySpecificEntryExitModel> jobCategorySpecificEntryExitModels=new List<JobCategorySpecificEntryExitModel>();
        }
        public int WorkCalendarId { get; set; }
        public DateTime EntryExitDate { get; set; }
        public string PersianEntryExitDate { get; set; }
        public string DayTitle { get; set; }
        public int DayNumber { get; set; }
        public string MonthNumber { get; set; } 
        public string YearNumber { get; set; }
        public bool IsExist { get; set; }
        public string EmployeeId { get; set; }
        public bool IsCreatedManual { get; set; }//دستی I
        public bool IsDeleted { get; set; }
        public bool IsHaveAttendItem { get; set; }




        public List<JobCategorySpecificEntryExitModel> jobCategorySpecificEntryExitModels { get; set; }
     
    }

}

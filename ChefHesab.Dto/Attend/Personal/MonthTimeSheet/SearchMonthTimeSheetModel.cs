using KSC.Common.Filters.Models;



namespace Ksc.HR.DTO.Personal.MonthTimeSheet
{
    public class SearchMonthTimeSheetModel 
    {
        public string DateTimeSheet { get; set; }
        public string CurrentUser { get; set; }
        public int Step { get; set; }
        public int ProcedureId { get; set; }
        public string Yearmonth { get; set; }
        //
        /// <summary>
        /// بازگشت مرحله به عقب
        /// </summary>
        public bool IsBackStep { get; set; }
        public int ResultCount { get; set; }
        public string Result { get; set; }
        public bool IsBackValid { get; set; }
        public bool IsActive { get; set; }
        public bool IsDone { get; set; }
        public bool? IsShowDetails { get; set; }
        public string UrlDetails { get; set; }
        public string Title { get; set; }
        public string AuthenticateUserName { get; set; }
        public int SystemSequenceControlId { get; set; }


    }
    public class SearchPivoteReport
    {
        public int StartYearMonth { get; set;}
        public int EndYearMonth { get; set; }

        public string RollCallId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItem

{
    public class ARREvaluation
    {
        public string COD_TYP { get; set; }//ارزیابی کد 3
        public string NUMP { get; set; }//شماره پرسنلی
        public string STIME { get; set; }//تاریخ شروع
        public string ETIME { get; set; }//تاریخ پایان
        /// <summary>
        ///   شناسایی ارزیابی توسعه  (شماره پرسنلی + تاریخ شروع کلاس)
        /// </summary>
        /// 
        public string FlgEvaluation { get; set; }

        public string FLG_ERR { get; set; }//اگر خطا رخ داد 1 می شود
        public string FLG_MIS { get; set; }//برای حذف از سیستم مرتبط 1 ارسال شود
        public string COD_ERR { get; set; }
        public string DES_ERR { get; set; }// شرح خطا
    }


    public class RPC005Evaluation
    {
        public string WinUser { get; set; }
        // public int TOT_CNT { get; set; }
        public string DATE { get; set; }//تاریخ کلاس
        public List<ARREvaluation> ARR { get; set; }
        public RPC005Evaluation()
        {
            ARR = new List<ARREvaluation>();
        }
    }
}

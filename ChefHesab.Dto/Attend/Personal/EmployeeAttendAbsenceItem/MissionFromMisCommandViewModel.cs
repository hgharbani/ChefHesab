using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.DTO.Personal.EmployeeAttendAbsenceItem
{
    public class PAR_ASSPY
    {
        //Data from MIs
        //public class MissionItems
        //{


        // public string ShamsiMissionStartDate { get; set; }

        // public string ShamsiMissionEndDate { get; set; }


        /// <summary>
        ///  شماره پرسنلی
        /// </summary>
        public string NUM_PRSN_EMPL { get; set; }
        // public string PAR_ASSPY { get; set; }

        /// <summary>
        /// نوع عملیات(R چک کردن داشتن ورود و خروج) (I ایجاد ماموریت) (D حذف کردن ماموریت)
        /// </summary>
        public string OPERATION { get; set; }

        /// <summary>
        ///   تاریخ شروع ماموریت
        /// </summary>
        /// 
        public string DAT_STR_ASSPY { get; set; }

        /// <summary>
        ///   شناسایی ماموریت  (شماره پرسنلی + تاریخ شروع ماموریت)
        /// </summary>
        /// 
        public string ASSPY_ID { get; set; }



        /// <summary>
        ///     تاریخ پایان ماموریت
        /// </summary>
        /// 
        public string DAT_END_ASSPY { get; set; }


        /// <summary>
        ///       کد حضور غیاب
        /// </summary>
        /// 
        public string FK_ATABT { get; set; }


        /// <summary>
        ///         کاربر ثبت کننده
        /// </summary>
        /// 
        public string COD_USR_ATABI { get; set; } // InsertUser (length: 50)

       // public string SUCCESS { get; set; }

       // public string STATUSCODE { get; set; }

       // public string CNT_ERROR { get; set; }

        //public DateTime? InsertDate { get; set; } // InsertDate
        //public DateTime? UpdateDate { get; set; } // UpdateDate
       // public string UpdateUser { get; set; } // UpdateUser (length: 50)
       // public string CurrentUserName { get; set; }

        //}
        //استراکچر استاندارد IT برای پاسخ درخواست سمت WEB
        //public class KSCRESULT 
        //{
        //public string SUCCESS { get; set; }
        //public string STATUSCODE { get; set; }
        //public string CNT_ERROR { get; set; }

       // public string KSCERROR { get; set; }
      // public string TITLE { get; set; }
       // public string MESSAGE { get; set; }
        //public string ERRORTYPE { get; set; }
        //}

        //استراکچر استاندارد IT برای خطا های سمت WEB
        //public class ERRORS
        //{
        //public string KSCERROR { get; set; }
        //public string TITLE { get; set; }
        //public string MESSAGE { get; set; }
        //public string ERRORTYPE { get; set; }

    }
}

//  }





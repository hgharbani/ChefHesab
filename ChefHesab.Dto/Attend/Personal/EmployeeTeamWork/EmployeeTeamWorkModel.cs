




using System;
using System.ComponentModel.DataAnnotations;

namespace Ksc.HR.DTO.Personal.EmployeeTeamWork
{
    public class EmployeeTeamWorkModel
    {
        // EmployeeTeamWork

        public int Id { get; set; } // Id (Primary key)

        /// <summary>
        /// شماره پرسنلی
        /// </summary>
        public int EmployeeId { get; set; } // EmployeeId

        /// <summary>
        /// گروه کاری
        /// </summary>
        [Display(Name = "تیم کاری ")]
        public int TeamWorkId { get; set; } // TeamWorkId

        /// <summary>
        /// تاریخ شروع
        /// </summary>
        /// 
        [Display(Name = "تاریخ شروع تیم ")]
        public DateTime TeamStartDate { get; set; } // TeamStartDate

        /// <summary>
        /// تاریخ پایان
        /// </summary>
        public DateTime? TeamEndDate { get; set; } // TeamEndDate
        public int? TransferRequestId { get; set; } // TransferRequestId
        public DateTime? InsertDate { get; set; } // InsertDate
        public string InsertUser { get; set; } // InsertUser (length: 50)
        public DateTime? UpdateDate { get; set; } // UpdateDate
        public string UpdateUser { get; set; } // UpdateUser (length: 50)
        public bool IsActive { get; set; } // IsActive
        public byte[] RowVersion { get; set; } // RowVersion (length: 8)
        public string NameFamily { get; set; } // NameFamily 
        public string TeamWorkTitle { get; set; } // TeamWorkTitle
        public string EmployeeNumber { get; set; } // TeamWorkTitle 
        public string TeamCode { get; set; } // TeamCode 
                                             // 








    }
}

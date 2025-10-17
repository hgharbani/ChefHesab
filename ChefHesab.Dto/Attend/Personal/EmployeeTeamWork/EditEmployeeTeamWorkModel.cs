

using System;

namespace Ksc.HR.DTO.Personal.EmployeeTeamWork
{
    public class EditEmployeeTeamWorkModel
    {
        public int Id { get; set; } // Id (Primary key)

        /// <summary>
        /// شماره پرسنلی
        /// </summary>
        public int EmployeeId { get; set; } // EmployeeId

        /// <summary>
        /// گروه کاری
        /// </summary>
        public int TeamWorkId { get; set; } // TeamWorkId

        /// <summary>
        /// تاریخ شروع
        /// </summary>
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

        // Foreign keys

        /// <summary>
        /// Parent Employee pointed by [EmployeeTeamWork].([EmployeeId]) (FK_EmployeeTeamWork_Employee)
        /// </summary>
        //public virtual Employee Employee { get; set; } // FK_EmployeeTeamWork_Employee

        /// <summary>
        /// Parent TeamWork pointed by [EmployeeTeamWork].([TeamWorkId]) (FK_EmployeeTeamWork_TeamWork)
        /// </summary>
        // public virtual TeamWork TeamWork { get; set; } // FK_EmployeeTeamWork_TeamWork

        /// <summary>
        /// Parent Transfer_Request pointed by [EmployeeTeamWork].([TransferRequestId]) (FK_EmployeeTeamWork_Request)
        /// </summary>
        // public virtual Transfer_Request Transfer_Request { get; set; } // FK_EmployeeTeamWork_Request
    }

}


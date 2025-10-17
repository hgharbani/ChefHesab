using Ksc.HR.Domain.Entities.HRSystemStatusControl;
using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Entities.Personal
{
    // EmployeeEfficiencyMonth
    /// <summary>
    /// ضریب کارایی ماه
    /// </summary>
    public class EmployeeEfficiencyMonth : IEntityBase<int>
    {
        public int Id { get; set; } // Id (Primary key)
        public int SystemSequenceStatusId { get; set; } // SystemSequenceStatusId

        /// <summary>
        /// تاریخ زمانبندی
        /// </summary>
        public int YearMonth { get; set; } // YearMonth
        public DateTime? InsertDate { get; set; } // InsertDate
        public string InsertUser { get; set; } // InsertUser (length: 50)

        /// <summary>
        /// کاربر جانشین
        /// </summary>
        public string InsertAuthenticateUserName { get; set; } // InsertAuthenticateUserName (length: 100)
        public DateTime? UpdateDate { get; set; } // UpdateDate
        public string UpdateUser { get; set; } // UpdateUser (length: 50)

        /// <summary>
        /// کاربر ویرایش کننده جانشین
        /// </summary>
        public string UpdateAuthenticateUserName { get; set; } // UpdateAuthenticateUserName (length: 100)
         public bool IsActive { get; set; } // IsActive

        // Foreign keys

        /// <summary>
        /// Parent SystemSequenceStatus pointed by [EmployeeEfficiencyMonth].([SystemSequenceStatusId]) (FK_Efficiency_SystemSequenceStatus)
        /// </summary>
        public virtual SystemSequenceStatus SystemSequenceStatus { get; set; } // FK_Efficiency_SystemSequenceStatus
    }
}

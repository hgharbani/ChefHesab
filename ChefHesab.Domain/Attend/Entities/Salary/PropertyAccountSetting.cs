using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Entities.Salary
{
    // PropertyAccountSetting
    /// <summary>
    /// واسط کدهای حساب ومشخصات حساب
    /// </summary>
    public class PropertyAccountSetting : IEntityBase<int>
    {
        public int Id { get; set; } // Id (Primary key)

        /// <summary>
        /// مشخصه حساب
        /// </summary>
        public int PropertyAccountId { get; set; } // PropertyAccountId

        /// <summary>
        /// کد حساب
        /// </summary>
        public int AccountCodeId { get; set; } // AccountCodeId

        /// <summary>
        /// تاریخ درج
        /// </summary>
        public DateTime? InsertDate { get; set; } // InsertDate

        /// <summary>
        /// کاربر درج کننده
        /// </summary>
        public string InsertUser { get; set; } // InsertUser (length: 50)

        /// <summary>
        /// تاریخ ویرایش
        /// </summary>
        public DateTime? UpdateDate { get; set; } // UpdateDate

        /// <summary>
        /// کاربر ویرایش کننده
        /// </summary>
        public string UpdateUser { get; set; } // UpdateUser (length: 50)

        /// <summary>
        /// فعال؟
        /// </summary>
        public bool IsActive { get; set; } // IsActive

        // Foreign keys

        /// <summary>
        /// Parent AccountCode pointed by [PropertyAccountSetting].([AccountCodeId]) (FK_PropertyAccountSetting_AccountCode)
        /// </summary>
        public virtual AccountCode AccountCode { get; set; } // FK_PropertyAccountSetting_AccountCode

        /// <summary>
        /// Parent PropertyAccount pointed by [PropertyAccountSetting].([PropertyAccountId]) (FK_PropertyAccountSetting_PropertyAccount)
        /// </summary>
        public virtual PropertyAccount PropertyAccount { get; set; } // FK_PropertyAccountSetting_PropertyAccount
    }
}

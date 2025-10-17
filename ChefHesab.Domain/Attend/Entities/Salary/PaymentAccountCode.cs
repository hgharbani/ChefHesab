using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Entities.Salary
{
    // PaymentAccountCode
    /// <summary>
    /// جدول واسط نوع پرداخت و کدحساب
    /// </summary>
    public class PaymentAccountCode:IEntityBase<int>
    {
        public int Id { get; set; } // Id (Primary key)

        /// <summary>
        /// کد حساب
        /// </summary>
        public int AccountCodeId { get; set; } // AccountCodeId

        /// <summary>
        /// نوع پرداختی
        /// </summary>
        public int PaymentTypeId { get; set; } // PaymentTypeId

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
        /// Parent AccountCode pointed by [PaymentAccountCode].([AccountCodeId]) (FK_PaymentAccountCode_AccountCode)
        /// </summary>
        public virtual AccountCode AccountCode { get; set; } // FK_PaymentAccountCode_AccountCode

        /// <summary>
        /// Parent PaymentType pointed by [PaymentAccountCode].([PaymentTypeId]) (FK_PaymentAccountCode_PaymentType)
        /// </summary>
        public virtual PaymentType PaymentType { get; set; } // FK_PaymentAccountCode_PaymentType
    }
}

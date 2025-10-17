using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Entities.Salary
{
    public class AccountCodeCompatible : IEntityBase<int>
    {
        public int Id { get; set; } // Id (Primary key)

        /// <summary>
        /// نوع کدهای حساب سازگار
        /// </summary>
        public int AccountCodeCompatibleTypeId { get; set; } // AccountCodeCompatibleTypeId

        /// <summary>
        /// کدهای حساب
        /// </summary>
        public int AccountCodeId { get; set; } // AccountCodeId
        public int AccountCodeCompatibleId { get; set; } // AccountCodeCompatibleId

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
        /// Parent AccountCode pointed by [AccountCodeCompatible].([AccountCodeId]) (FK_AccountCodeCompatible_AccountCode)
        /// </summary>
        public virtual AccountCode AccountCode { get; set; } // FK_AccountCodeCompatible_AccountCode

        /// <summary>
        /// Parent AccountCode pointed by [AccountCodeCompatible].([AccountCodeCompatibleId]) (FK_AccountCodeCompatible_AccountCode1)
        /// </summary>
        public virtual AccountCode Salary_AccountCodeCompatible { get; set; } // FK_AccountCodeCompatible_AccountCode1

        /// <summary>
        /// Parent AccountCodeCompatibleType pointed by [AccountCodeCompatible].([AccountCodeCompatibleTypeId]) (FK_AccountCodeCompatible_AccountCodeCompatibleType)
        /// </summary>
        public virtual AccountCodeCompatibleType AccountCodeCompatibleType { get; set; } // FK_AccountCodeCompatible_AccountCodeCompatibleType
    }
}

using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Entities.Salary
{
    public class AccountCodeCompatibleType : IEntityBase<int>
    {
        public int Id { get; set; } // Id (Primary key)

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } // Title

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

        // Reverse navigation

        // <summary>
        // Child AccountCodeCompatibles where[AccountCodeCompatible].[AccountCodeCompatibleTypeId] point to this entity(FK_AccountCodeCompatible_AccountCodeCompatibleType)
        // </summary>
        public virtual ICollection<AccountCodeCompatible> AccountCodeCompatibles { get; set; } // AccountCodeCompatible.FK_AccountCodeCompatible_AccountCodeCompatibleType

        public AccountCodeCompatibleType()
        {
            AccountCodeCompatibles = new List<AccountCodeCompatible>();
        }
    }
}

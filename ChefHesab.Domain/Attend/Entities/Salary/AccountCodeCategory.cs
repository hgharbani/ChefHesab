using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Entities.Salary
{
    public class AccountCodeCategory : IEntityBase<int>
    {
        public int Id { get; set; } // Id (Primary key)

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } // Title
        public int? ParentId { get; set; } // ParentId

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

        /// <summary>
        /// Child AccountCodes where [AccountCode].[AccountCodeCategoryId] point to this entity (FK_AccountCode_AccountCodeCategory)
        /// </summary>
        public virtual ICollection<AccountCode> AccountCodes { get; set; } // AccountCode.FK_AccountCode_AccountCodeCategory

        /// <summary>
        /// Child AccountCodeCategories where [AccountCodeCategory].[ParentId] point to this entity (FK_AccountCodeCategory_AccountCodeCategory)
        /// </summary>
        public virtual ICollection<AccountCodeCategory> AccountCodeCategories { get; set; } // AccountCodeCategory.FK_AccountCodeCategory_AccountCodeCategory

        // Foreign keys

        /// <summary>
        /// Parent AccountCodeCategory pointed by [AccountCodeCategory].([ParentId]) (FK_AccountCodeCategory_AccountCodeCategory)
        /// </summary>
        public virtual AccountCodeCategory Parent { get; set; } // FK_AccountCodeCategory_AccountCodeCategory

        public AccountCodeCategory()
        {
            AccountCodes = new List<AccountCode>();
            AccountCodeCategories = new List<AccountCodeCategory>();
        }
    }
}

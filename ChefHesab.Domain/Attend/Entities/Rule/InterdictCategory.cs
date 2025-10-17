using Ksc.HR.Domain.Entities.Salary;
using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Entities.Rule
{
    public class InterdictCategory:IEntityBase<int>
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


        public bool IsEditablePrice { get; set; } // IsEditablePrice
        // Reverse navigation

        /// <summary>
        /// Child Salary_AccountCodes where [AccountCode].[InterdictCategoryId] point to this entity (FK_AccountCode_InterdictCategory)
        /// </summary>
        public virtual ICollection<AccountCode> AccountCodes { get; set; } // AccountCode.FK_AccountCode_InterdictCategory

        public InterdictCategory()
        {
            AccountCodes = new List<AccountCode>();
        }
    }
}

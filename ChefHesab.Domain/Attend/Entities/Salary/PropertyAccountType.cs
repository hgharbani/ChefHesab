using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Entities.Salary
{
    public class PropertyAccountType : IEntityBase<int>
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

        /// <summary>
        /// Child PropertyAccounts where [PropertyAccount].[PropertyAccountTypeId] point to this entity (FK_PropertyAccount_PropertyAccountType)
        /// </summary>
        public virtual ICollection<PropertyAccount> PropertyAccounts { get; set; } // PropertyAccount.FK_PropertyAccount_PropertyAccountType

        public PropertyAccountType()
        {
            PropertyAccounts = new List<PropertyAccount>();
        }
    }
}

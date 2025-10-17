using Ksc.HR.Domain.Entities.Personal;
using Ksc.HR.Domain.Entities.Rule;
using KSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksc.HR.Domain.Entities.EmployeeBase
{
    // EmploymentType
    public class EmploymentType : IEntityBase<int>
    {
        /// <summary>
        /// شناسه
        /// </summary>
        public int Id { get; set; } // Id (Primary key)

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } // Title (length: 50)

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
        public bool? IsActive { get; set; } // IsActive
        /// <summary>
        /// تام شیت ماهیانه بصورت دستی ساخته میشود؟
        /// </summary>
        public bool IsCreatedManualMonthTimeSheet { get; set; } // IsCreatedManualMonthTimeSheet

        // Reverse navigation

        /// <summary>
        /// Child Employees where [Employee].[EmploymentTypeId] point to this entity (FK_Employee_EmploymentType)
        /// </summary>
        public virtual ICollection<Employee> Employees { get; set; } // Employee.FK_Employee_EmploymentType
        public virtual ICollection<EmployeeInterdict> EmployeeInterdicts { get; set; } // EmployeeInterdict.FK_EmployeeInterdict_JobPosition
        public virtual ICollection<AccountEmploymentType> AccountEmploymentTypes { get; set; } // EmployeeInterdict.FK_EmployeeInterdict_JobPosition
        public virtual ICollection<BasisSalaryItem> BasisSalaryItems { get; set; } // EmployeeInterdict.FK_EmployeeInterdict_JobPosition
        public virtual ICollection<InterdictMaritalSetting> InterdictMaritalSettings { get; set; } // InterdictMaritalSettingDetail.FK_InterdictMaritalSettingDetail_MaritalStatus
        public virtual ICollection<IncreaseSalaryDetail> IncreaseSalaryDetails { get; set; } // 


        public EmploymentType()
        {
            Employees = new List<Employee>();
            EmployeeInterdicts = new List<EmployeeInterdict>();
            AccountEmploymentTypes = new List<AccountEmploymentType>();
            BasisSalaryItems = new List<BasisSalaryItem>();
            InterdictMaritalSettings = new List<InterdictMaritalSetting>();
            IncreaseSalaryDetails = new List<IncreaseSalaryDetail>();
        }
    }
}

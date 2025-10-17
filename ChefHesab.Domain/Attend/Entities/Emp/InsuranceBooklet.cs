using Ksc.HR.Domain.Entities.Personal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ksc.HR.Domain.Entities.Workshift;
using KSC.Domain;
using Ksc.HR.Domain.Entities.EmployeeBase;

namespace Ksc.HR.Domain.Entities.Emp
{
    public class InsuranceBooklet : IEntityBase<long>

    {
        public long Id { get; set; } // Id (Primary key)

        /// <summary>
        /// شماره دفترچه
        /// </summary>
        public string BookletNumber { get; set; } // BookletNumber

        /// <summary>
        /// المثنی؟
        /// </summary>
        public bool IsDuplicate { get; set; } // IsDuplicate

        /// <summary>
        /// شماره المثنی
        /// </summary>
        public string DuplicateNumber { get; set; } // DuplicateNumber (length: 10)
        public DateTime? DuplicateDate { get; set; } // DuplicateDate

        /// <summary>
        /// تاریخ صدور
        /// </summary>
        public DateTime? IssueDate { get; set; } // IssueDate

        /// <summary>
        /// شماره سریال
        /// </summary>
        public string StartSerialNumber { get; set; } // StartSerialNumber
        public string ExitSerialNumber { get; set; } // ExitSerialNumber

        /// <summary>
        /// سال چاپ
        /// </summary>
        public string PrintYear { get; set; } // PrintYear
        public string Description { get; set; } // PrintYear

        /// <summary>
        /// تاریخ اعتبار
        /// </summary>
        public DateTime? StartValidDate { get; set; } // StartValidDate

        /// <summary>
        /// تاریخ خاتمه بیمه
        /// </summary>
        public DateTime? EndValidDate { get; set; } // EndValidDate

        /// <summary>
        /// فعال؟
        /// </summary>
        public bool IsActive { get; set; } // IsActive


        public string InsertUser { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        /// <summary>
        /// شهر محل صدور دفترچه
        /// </summary>
        public int BookletIssuingCityId { get; set; } // BookletIssuingCityId

        /// <summary>
        /// نوع فرانشیز
        /// </summary>
        public int FranchiseTypeId { get; set; } // FranchiseTypeId

        /// <summary>
        /// میزان فرانشیز
        /// </summary>
        public double FranchiseAmount { get; set; } // FranchiseAmount

        /// <summary>
        /// شناسه پرسنلی
        /// </summary>
        public int? EmployeeId { get; set; } // EmployeeId

        /// <summary>
        /// شناسه تحت تکفل
        /// </summary>
        public int? FamilyId { get; set; } // FamilyId

        /// <summary>
        /// شناسه افراد قبل mis
        /// </summary>
        public int? PensionerId { get; set; } // PensionerId
        public long? ParentId { get; set; } // ParentId


        public virtual ICollection<InsuranceBooklet> InsuranceBooklets { get; set; } // InsuranceBooklet.FK_InsuranceBooklet_InsuranceBooklet


        public virtual City BookletIssuingCity { get; set; } // FK_InsuranceBooklet_City


        public virtual Family Family { get; set; } // FK_InsuranceBooklet_Family


        public virtual InsuranceBooklet Parent { get; set; } // FK_InsuranceBooklet_InsuranceBooklet

        public virtual Employee Employee { get; set; } // FK_InsuranceBooklet_Employee


        public virtual FranchiseType FranchiseType { get; set; } // FK_InsuranceBooklet_FranchiseType

        public InsuranceBooklet()
        {
            InsuranceBooklets = new List<InsuranceBooklet>();
        }
    }
}

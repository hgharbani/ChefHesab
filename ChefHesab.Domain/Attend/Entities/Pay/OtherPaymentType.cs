using Ksc.HR.Domain.Entities.Emp;
using KSC.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Ksc.HR.Domain.Entities.Pay
{
    public class OtherPaymentType : IEntityBase<int>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        //public bool IsValidPaymnetDelete { get; set; }
        //public bool IsValidRepeatableInPaymnetPeriod { get; set; }
        public bool IsActive { get; set; }
        public string InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public virtual ICollection<EmployeeOtherPaymentType> EmployeeOtherPaymentType { get; set; }
        public virtual ICollection<OtherPaymentSetting> OtherPaymentSetting { get; set; }
        // Reverse navigation

        /// <summary>
        /// Child Pay_OtherPaymentHeaderTypes where [OtherPaymentHeaderType].[OtherPaymentTypeId] point to this entity (FK_OtherPaymentHeaderType_OtherPaymentType)
        /// </summary>
        public virtual ICollection<OtherPaymentHeaderType> OtherPaymentHeaderTypes { get; set; } // OtherPaymentHeaderType.FK_OtherPaymentHeaderType_OtherPaymentType
        public virtual ICollection<EmployeeBlackList> EmployeeBlackLists { get; set; } // EmployeeBlackList.FK_EmployeeBlackList_Employee


        public OtherPaymentType()
        {
            EmployeeBlackLists = new List<EmployeeBlackList>();
            OtherPaymentHeaderTypes = new List<OtherPaymentHeaderType>();
            OtherPaymentSetting = new List<OtherPaymentSetting>();
        }
    }
}


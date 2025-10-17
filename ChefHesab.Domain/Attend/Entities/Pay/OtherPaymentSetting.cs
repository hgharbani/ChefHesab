using Ksc.HR.Domain.Entities.Salary;
using KSC.Domain;
namespace Ksc.HR.Domain.Entities.Pay
{
    public class OtherPaymentSetting : IEntityBase<int>
    {
        public int Id { get; set; }
        public int OtherPaymentTypeId { get; set; }
        public int AccountCodeId { get; set; }
        public int ValidityStartYearMonth { get; set; }
        public int? ValidityEndYearMonth { get; set; }

        /// <summary>
        /// این حذف شده
        /// </summary>
        public int? KPercent { get; set; }
        //public int? KUnit { get; set; }
        //public int? CountValidDaysForPayment { get; set; }
        //public int? MinimumPersonForPayment { get; set; }
        //public int? MaximumPersonForPayment { get; set; }
        public bool IsActive { get; set; }
        public string InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual ICollection<OtherPaymentSettingParameterValue> OtherPaymentSettingParameterValues { get; set; } // OtherPaymentSettingParameterValue.FK_OtherPaymentSettingParameterValue_OtherPaymentSettingParameter
        public virtual OtherPaymentType OtherPaymentType { get; set; }
        public virtual AccountCode AccountCode { get; set; }
    }
}


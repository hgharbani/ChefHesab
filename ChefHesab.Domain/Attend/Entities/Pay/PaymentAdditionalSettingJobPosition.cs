using Ksc.HR.Domain.Entities.Chart;
using KSC.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Ksc.HR.Domain.Entities.Pay
{
    public class PaymentAdditionalSettingJobPosition : IEntityBase<int>
    {
        public int Id { get; set; }
        public int PaymentAdditionalSettingId { get; set; }
        public int JobpositionId { get; set; }
        public int ValidityStartYearMonth { get; set; }
        public int? ValidityEndYearMonth { get; set; }
        public long BaseAmount { get; set; }
        public bool IsActive { get; set; }
        public string InsertUser { get; set; }
        public DateTime InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public virtual PaymentAdditionalSetting PaymentAdditionalSetting { get; set; }
        public virtual Chart_JobPosition JobPosition { get; set; }
    }
}


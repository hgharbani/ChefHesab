using KSC.Domain;
namespace Ksc.HR.Domain.Entities.View
{
    public class View_Employee : IEntityBase
    {
        public int EmployeeId { get; set; }
        public string WindowsUser { get; set; }
        public string EmployeeNumber { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string PhoneNumber { get; set; }
        public int? TeamWorkId { get; set; }
        public string TeamCode { get; set; }
        public string TeamTitle { get; set; }
        public string MisJobPositionCode { get; set; }
        public string JobPositionTitle { get; set; }
        public int? PaymentStatusId { get; set; }
        public string PaymentStatusTitle { get; set; }
        public int? EmploymentTypeId { get; set; }
        public string EmploymentTypeTitle { get; set; }
        public string JobCategoryCode { get; set; }
        public string JobCategoryTitle { get; set; }
        public decimal? CostCenter { get; set; }

        public int? JobPositionId { get; set; }

        public int? JobStatusCode { get; set; }
        public string JobStatusDescription { get; set; }

        public int? CategoryId { get; set; }
        public string CategoryTitle { get; set; }
        public string MisUser { get; set; }
        public string NationalCode { get; set; }
    }
}


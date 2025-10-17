using KSC.Domain;
namespace Ksc.Hr.Domain.Entities
{
    public class View_PresentEmployees : IEntityBase
    {
        public long? Id { get; set; }
        public int? EmployeeId { get; set; }
        public string EmployeeNumber { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public int? TeamWorkId { get; set; }
        public int? JobPositionId { get; set; }
        public bool? WorkDay { get; set; }
        public bool? IsPresent { get; set; }
        public string TeamWorkCode { get; set; }
        public string TeamWorkTitle { get; set; }
        public string MisJobPositionCode { get; set; }
        public string JobPositionTitle { get; set; }
        public decimal? CostCenter { get; set; }
        public bool? IsKscEmployee { get; set; }
        public string ContractCode { get; set; }
        public string ContractDescription { get; set; }
        public string Contractor { get; set; }
        public decimal? ContractorID { get; set; }
    }
}

